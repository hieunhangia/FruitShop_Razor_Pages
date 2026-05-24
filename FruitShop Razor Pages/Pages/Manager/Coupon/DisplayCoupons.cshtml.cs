using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs;
using Service.DTOs.Manager;

namespace FruitShop_Razor_Pages.Pages.Manager.Coupon;

[Authorize(Roles = Role.Manager)]
public class IndexModel(Service.Manager.CouponService couponService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<CouponFilter> PagedAndSortedRequest { get; set; } = new();

    public required PagedAndSortedDto<CouponDto> PagedAndSortedResult { get; set; }

    public async Task<IActionResult> OnGetAsync(bool? isSearch)
    {
        if (!ModelState.IsValid)
        {
            PagedAndSortedResult = new PagedAndSortedDto<CouponDto>([], 0, 0, 0, string.Empty, SortDirection.Ascending);
            return Page();
        }

        if (isSearch == true)
        {
            PagedAndSortedRequest.PageIndex = 1;
        }

        PagedAndSortedResult = await couponService.GetAllCouponsAsync(PagedAndSortedRequest);
        return Page();
    }
}