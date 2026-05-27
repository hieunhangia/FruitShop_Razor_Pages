using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs;
using Service.DTOs.SalesStaff.Order;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Order;

[Authorize(Roles = Role.SalesStaff)]
public class View(OrderService service) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<OrderFilter> PagedAndSortedRequest { get; set; } = new();

    public PagedAndSortedDto<OrderListDto>? PagedAndSortedResult { get; set; }

    public async Task<IActionResult> OnGetAsync(bool? isSearch)
    {
        if (!ModelState.IsValid)
        {
            PagedAndSortedResult = new PagedAndSortedDto<OrderListDto>([], 0, 0, 0,
                "", SortDirection.Ascending);
            return Page();
        }

        if (isSearch == true)
        {
            PagedAndSortedRequest.PageIndex = 1;
        }

        PagedAndSortedResult = await service.GetOrderListAsync(PagedAndSortedRequest);
        return Page();
    }
}
