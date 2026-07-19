using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.Customer;
using Service.DTOs.Customer;

namespace FruitShop_Razor_Pages.Pages.Customer.Dashboard;

[Authorize(Roles = Role.Customer)]
public class IndexModel(CustomerDashboardService customerDashboardService) : PageModel
{
    public CustomerDashboardDto DashboardData { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var customerId))
        {
            ViewData["ErrorMessage"] = "Không xác định được tài khoản.";
            return Page();
        }

        DashboardData = await customerDashboardService.GetDashboardDataAsync(customerId);
        return Page();
    }
}
