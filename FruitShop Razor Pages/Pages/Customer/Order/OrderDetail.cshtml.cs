using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.Customer;
using Service.DTOs.Customer.Order;

namespace FruitShop_Razor_Pages.Pages.Customer.Order;

[Authorize(Roles = Role.Customer)]
public class OrderDetailModel(OrderService orderService) : PageModel
{
    public OrderDetailDto? Order { get; set; }

    public async Task<IActionResult> OnGetAsync(long id)
    {
        try
        {
            var customerId = User.GetUserId();
            Order = await orderService.GetOrderDetailAsync(customerId, id);
            return Page();
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }
    }

    public async Task<IActionResult> OnPostCancelCashOnDeliveryAsync(long id)
    {
        try
        {
            var customerId = User.GetUserId();
            await orderService.CancelCashOnDeliveryOrderAsync(customerId, id);
            TempData["SuccessMessage"] = "Hủy đơn hàng thành công.";
            return RedirectToPage();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return RedirectToPage();
        }
    }

    public async Task<IActionResult> OnPostCancelQRCodePaymentAsync(long id)
    {
        try
        {
            var customerId = User.GetUserId();
            await orderService.CancelQrCodePaymentOrderByCustomerAsync(customerId, id);
            TempData["SuccessMessage"] = "Hủy đơn hàng thành công.";
            return RedirectToPage();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return RedirectToPage();
        }
    }
}