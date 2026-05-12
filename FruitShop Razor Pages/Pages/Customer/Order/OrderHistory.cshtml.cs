using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Repository.Models.Users;
using Service.Customer;
using Service.DTOs;
using Service.DTOs.Customer.Order;

namespace FruitShop_Razor_Pages.Pages.Customer.Order;

[Authorize(Roles = Role.Customer)]
public class OrderHistoryModel(OrderService orderService, UserManager<User> userManager) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<OrderFilter> PagedAndSortedRequest { get; set; } = new();

    public PagedAndSortedDto<OrderSummaryDto> PagedAndSortedResult { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(bool? isSearch)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (isSearch == true)
        {
            PagedAndSortedRequest.PageIndex = 1;
        }

        var customerId = int.Parse(userManager.GetUserId(User)!);
        PagedAndSortedResult = await orderService.GetOrderHistoryListAsync(customerId, PagedAndSortedRequest);
        return Page();
    }
}