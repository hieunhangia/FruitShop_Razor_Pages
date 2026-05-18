using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Models.Users;
using Service.DTOs.Customer.Order;
using Service.Shipper;

namespace FruitShop_Razor_Pages.Pages.Shipper
{
    public class ShipperOrdersModel(ShipperOrderService shipperOrderService, UserManager<User> userManager) : PageModel
    {
        public List<OrderSummaryDto> Orders { get; set; } = [];
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);

            if(user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Orders = await shipperOrderService.GetAvailableOrdersForShipper(user.Id);
            return Page();
        }
    }
}
