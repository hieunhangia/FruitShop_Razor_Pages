using Microsoft.AspNetCore.Authorization;
using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.Constants;
using Service.Customer;
using Service.DTOs;
using Service.DTOs.Customer.Coupon;
using Service.DTOs.Manager;

namespace FruitShop_Razor_Pages.Pages.Customer.Coupon;

[Authorize(Roles = Role.Customer)]
public class ShopModel(CouponService service) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<CouponFilter> PagedAndSortedRequest { get; set; } = new();

    public required PagedAndSortedDto<CouponShopDto> PagedAndSortedResult { get; set; }

    public required long LoyaltyScore { get; set; }

    public async Task OnGetAsync(bool? isSearch)
    {
        var customerId = User.GetUserId();
        LoyaltyScore = await service.GetLoyaltyPoints(customerId);
        if (!ModelState.IsValid)
        {
            PagedAndSortedResult =
                new PagedAndSortedDto<CouponShopDto>([], 0, 0, 0, string.Empty,
                    SortDirection.Ascending);
            return;
        }

        if (isSearch == true)
        {
            PagedAndSortedRequest.PageIndex = BusinessRuleConstants.CouponPageValue.DefaultIndex;
        }

        PagedAndSortedRequest.PageSize = BusinessRuleConstants.CouponPageValue.NumberOfElement;
        PagedAndSortedResult = await service.GetAllAvailableCouponsForSaleAsync(PagedAndSortedRequest);
    }

    public async Task<IActionResult> OnGetBuyAsync(int couponId)
    {
        try
        {
            var customerId = User.GetUserId();
            await service.BuyCoupon(couponId, customerId);
            TempData["SuccessMessage"] = "Đã mua coupon thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage();
    }
}