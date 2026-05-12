using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Repository.Models.Users;
using Service.Customer;

namespace FruitShop_Razor_Pages.Pages.Customer.Order;

[Authorize(Roles = Role.Customer)]
public class QrCodePaymentOrderCancelledModel(OrderService orderService, UserManager<User> userManager) : PageModel
{
    public async Task<IActionResult> OnGetAsync(long? orderCode)
    {
        if (orderCode == null)
        {
            return NotFound();
        }

        try
        {
            var customerId = int.Parse(userManager.GetUserId(User)!);
            await orderService.CancelQrCodePaymentOrderAsync(customerId, orderCode.Value);
            return Page();
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }
    }
}