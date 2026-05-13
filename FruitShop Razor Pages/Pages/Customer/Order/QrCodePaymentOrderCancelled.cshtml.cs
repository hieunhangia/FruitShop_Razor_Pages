using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Customer;

namespace FruitShop_Razor_Pages.Pages.Customer.Order;

public class QrCodePaymentOrderCancelledModel(OrderService orderService) : PageModel
{
    public async Task<IActionResult> OnGetAsync(long orderCode)
    {
        try
        {
            await orderService.CancelQrCodePaymentOrderByPayOsCallbackAsync(orderCode);
            return Page();
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }
    }
}