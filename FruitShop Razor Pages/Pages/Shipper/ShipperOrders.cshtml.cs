using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Repository.Models.Users;
using Service.DTOs.Customer.Order;
using Service.Shipper;

namespace FruitShop_Razor_Pages.Pages.Shipper
{
    [Authorize(Roles = Role.Shipper)]
    public class ShipperOrdersModel(OrderService shipperOrderService, UserManager<User> userManager) : PageModel
    {
        public List<OrderSummaryDto> Orders { get; set; } = [];
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.GetUserId();

            Orders = await shipperOrderService.GetAvailableOrdersForShipper(userId);
            return Page();
        }
    }
}
