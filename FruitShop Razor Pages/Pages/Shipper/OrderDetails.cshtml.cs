using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Repository.Models.Users;
using Service.DTOs.Customer.Order;
using Service.Shipper;

namespace FruitShop_Razor_Pages.Pages.Shipper
{
    public class OrderDetailsModel(OrderService shipperOrderService, UserManager<User> userManager) : PageModel
    {
        public OrderDetailDto orderDetail { get; set; } = null;
        [TempData]
        public string? StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(long id)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToPage("/Account/Login");
                }
                orderDetail = await shipperOrderService.GetShipperOrderDetailAsync(id);
                return Page();
            }
            catch (Exception e)
            {
                return RedirectToPage("./ShipperOrders");
            }

        }

        public async Task<IActionResult> OnPostCompleteAsync(long id)
        {
            try
            {
                await shipperOrderService.AdvanceShippingStatusAsync(id);
                StatusMessage = "Cập nhật đơn hàng thành công!";
                return RedirectToPage("/Shipper/OrderDetails", new { id } );           

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                orderDetail = await shipperOrderService.GetShipperOrderDetailAsync(id);
                return Page();
            }
        }
    }
}
