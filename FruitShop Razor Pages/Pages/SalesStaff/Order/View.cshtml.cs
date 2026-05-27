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
    public required PagedAndSortedRequest<OrderFilter> PagedAndSortedRequest { get; set; }

    public required PagedAndSortedDto<OrderListDto> PagedAndSortedResult { get; set; }

    public async Task<IActionResult> OnGetAsync(PagedAndSortedRequest<OrderFilter> request)
    {
        PagedAndSortedResult = await service.GetOrderListAsync(request);
        return Page();
    }
}
