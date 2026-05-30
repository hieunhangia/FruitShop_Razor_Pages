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

        public async Task<IActionResult> OnGetAsync(bool? isSearch)
        {
            if (RequestData.Filter is { FromDate: not null, ToDate: not null })
            {
                if (RequestData.Filter.FromDate > RequestData.Filter.ToDate)
                {
                    ModelState.AddModelError(string.Empty, "Từ ngày tìm kiếm không được lớn hơn Đến ngày.");
                }
            }

            if (!ModelState.IsValid)
            {
                PagedResult = new PagedAndSortedDto<OrderSummaryDto>([], 0, 1, 10,
                    nameof(Repository.Models.Orders.Order.OrderDate), SortDirection.Descending);
                return Page();
            }

            if (isSearch == true)
            {
                RequestData.PageIndex = 1;
            }

            if (string.IsNullOrEmpty(RequestData.SortColumn))
            {
                RequestData.SortColumn = nameof(Repository.Models.Orders.Order.OrderDate);
                RequestData.SortDirection = SortDirection.Descending;
            }

            var shipperId = User.GetUserId();

            PagedResult = await shipperOrderService.GetPagedOrdersForShipperAsync(shipperId, RequestData);
            return Page();
        }
    }
}