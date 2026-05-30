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
    public class ShipperHistoryModel(OrderService orderService) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public PagedAndSortedRequest<OrderFilter> PagedAndSortedRequest { get; set; } = new();

        public PagedAndSortedDto<OrderSummaryDto>? PagedAndSortedResult { get; set; }

        public async Task<IActionResult> OnGetAsync(bool? isSearch)
        {
            if (PagedAndSortedRequest.Filter is { StartDate: not null, EndDate: not null })
            {
                if (PagedAndSortedRequest.Filter.StartDate > PagedAndSortedRequest.Filter.EndDate)
                {
                    ModelState.AddModelError(string.Empty, "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");
                }
            }

            if (!ModelState.IsValid)
            {
                PagedAndSortedResult = new PagedAndSortedDto<OrderSummaryDto>([], 0, 1, 10,
                    nameof(Repository.Models.Orders.Order.OrderDate), SortDirection.Descending);
                return Page();
            }

            if (isSearch == true)
            {
                PagedAndSortedRequest.PageIndex = 1;
            }

            var customerId = User.GetUserId();

            PagedAndSortedResult = await orderService.GetOrderHistoryListAsync(customerId, PagedAndSortedRequest);
            return Page();
        }
    }
}
