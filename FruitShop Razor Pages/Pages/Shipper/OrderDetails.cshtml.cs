using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Models.Users;
using Service.DTOs.Customer.Order;
using Service.Shipper;

namespace FruitShop_Razor_Pages.Pages.Shipper
{
    public class OrderDetailsModel(ShipperOrderService shipperOrderService, UserManager<User> userManager) : PageModel
    {
        public OrderDetailDto orderDetail { get; set; } = null; 
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
    }
}
