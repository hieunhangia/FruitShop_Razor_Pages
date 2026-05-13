using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Repository.Models.Users;
using Service.Customer;
using Service.DTOs.Customer.Order;

namespace FruitShop_Razor_Pages.Pages.Customer.Order;

[Authorize(Roles = Role.Customer)]
public class OrderDetailModel(OrderService orderService, UserManager<User> userManager) : PageModel
{
    public OrderDetailDto Order { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(long id)
    {
        try
        {
            var customerId = int.Parse(userManager.GetUserId(User)!);
            Order = await orderService.GetOrderDetailAsync(customerId, id);
            return Page();
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }
    }
}