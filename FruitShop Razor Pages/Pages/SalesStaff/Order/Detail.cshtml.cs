using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff.Order;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Order;

[Authorize(Roles = Role.SalesStaff)]
public class Detail(OrderService service) : PageModel
{
    public OrderDetailDto? Order { get; set; }

    public async Task<IActionResult> OnGetAsync(long id)
    {
        try
        {
            Order = await service.GetOrderDetailByIdAsync(id);
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
        }

        return Page();
    }
}
