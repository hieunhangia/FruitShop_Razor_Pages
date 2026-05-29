using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Repository.Models.Users;
using Service.DTOs;
using Service.DTOs.Customer.Order;
using Service.DTOs.Shipper;
using Service.Shipper;

namespace FruitShop_Razor_Pages.Pages.Shipper
{
    [Authorize(Roles = Role.Shipper)]
    public class ShipperOrdersModel(OrderService shipperOrderService, UserManager<User> userManager) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public PagedAndSortedRequest<OrderFilterDto> RequestData { get; set; } = new();

        public PagedAndSortedDto<OrderSummaryDto> PagedResult { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.GetUserId();

            if (string.IsNullOrEmpty(RequestData.SortColumn))
            {
                RequestData.SortColumn = "OrderDate";
                RequestData.SortDirection = SortDirection.Descending;
            }

            PagedResult = await shipperOrderService.GetPagedOrdersForShipperAsync(userId, RequestData);
            return Page();
        }
    }
}