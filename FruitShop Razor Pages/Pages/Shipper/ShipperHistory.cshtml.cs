using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs;
using Service.DTOs.Customer.Order;
using Service.DTOs.Shipper;
using Service.Shipper;

namespace FruitShop_Razor_Pages.Pages.Shipper
{
    [Authorize(Roles = "Shipper")]
    public class ShipperHistoryModel(OrderService shipperOrderService) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public PagedAndSortedRequest<OrderFilterDto> RequestData { get; set; } = new();

        public PagedAndSortedDto<OrderSummaryDto> PagedResult { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.GetUserId();

            if (string.IsNullOrEmpty(RequestData.SortColumn))
            {
                RequestData.SortColumn = "orderdate";
                RequestData.SortDirection = SortDirection.Descending;
            }

            // Gọi hàm lấy đơn ĐÃ GIAO THÀNH CÔNG mới viết ở Bước 1
            PagedResult = await shipperOrderService.GetDeliveredOrdersForShipperAsync(userId, RequestData);

            return Page();
        }
    }
}
