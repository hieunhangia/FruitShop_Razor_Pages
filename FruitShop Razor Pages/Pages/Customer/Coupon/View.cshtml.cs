using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.Customer;
using Service.DTOs;
using Service.DTOs.Customer.Coupon;
using Service.DTOs.Manager;

namespace FruitShop_Razor_Pages.Pages.Customer.Coupon;

[Authorize(Roles = Role.Customer)]
public class View(CouponService service) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<CouponFilter> PagedAndSortedRequest { get; set; } = new();
    public required PagedAndSortedDto<CouponViewDto> PagedAndSortedResult { get; set; }
    public async Task OnGetAsync(bool? isSearch)
    {
        var customerId = User.GetUserId();
        if (!ModelState.IsValid)
        {
            PagedAndSortedResult =
                new PagedAndSortedDto<CouponViewDto>([], 0, 0, 0, string.Empty,
                    SortDirection.Ascending);
            return;
        }
        if (isSearch == true)
        {
            PagedAndSortedRequest.PageIndex = 1;
        }
        PagedAndSortedResult = await service.GetAvailableCouponsForViewAsync(customerId,PagedAndSortedRequest);
    }
}