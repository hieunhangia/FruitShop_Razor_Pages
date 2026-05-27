using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.Constants;
using Service.Everyone;
using Service.DTOs;
using Service.DTOs.Everyone.Coupon;
using Service.DTOs.Manager;

namespace FruitShop_Razor_Pages.Pages.Everyone.Coupon;

public class ShopModel(CouponService service) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<CouponFilter> PagedAndSortedRequest { get; set; } = new();

    public required PagedAndSortedDto<CouponShopDto> PagedAndSortedResult { get; set; }

    public long CustomerLoyaltyPoints { get; set; }

    public async Task<IActionResult> OnGetAsync(bool? isSearch)
    {
        if (User.IsAuthenticated())
        {
            if (!User.IsInRole(Role.Customer))
            {
                return Forbid();
            }

            var customerId = User.GetUserId();
            CustomerLoyaltyPoints = await service.GetLoyaltyPoints(customerId);
        }

        if (!ModelState.IsValid)
        {
            PagedAndSortedResult =
                new PagedAndSortedDto<CouponShopDto>([], 0, 0, 0, string.Empty,
                    SortDirection.Ascending);
            return Page();
        }

        if (isSearch == true)
        {
            PagedAndSortedRequest.PageIndex = BusinessRuleConstants.CouponPageValue.DefaultIndex;
        }

        PagedAndSortedRequest.PageSize = BusinessRuleConstants.CouponPageValue.NumberOfElement;
        PagedAndSortedResult = await service.GetAllAvailableCouponsForSaleAsync(PagedAndSortedRequest);
        return Page();
    }

    public async Task<IActionResult> OnPostBuyAsync(int couponId)
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