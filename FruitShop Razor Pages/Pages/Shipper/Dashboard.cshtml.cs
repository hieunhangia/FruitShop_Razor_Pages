using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.Shipper;
using Service.Shipper;

namespace FruitShop_Razor_Pages.Pages.Shipper;

[Authorize(Roles = Role.Shipper)]
public class DashboardModel(ShipperDashboardService shipperDashboardService) : PageModel
{
    public ShipperDashboardDto DashboardData { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            ViewData["ErrorMessage"] = "Không xác định được tài khoản.";
            return Page();
        }

        DashboardData = await shipperDashboardService.GetDashboardDataAsync(userId);
        return Page();
    }
}

