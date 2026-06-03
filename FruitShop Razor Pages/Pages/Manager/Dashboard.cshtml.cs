using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.Manager;
using Service.Manager;

namespace FruitShop_Razor_Pages.Pages.Manager;

[Authorize(Roles = Role.Manager)]
public class DashboardModel(ManagerDashboardService dashboardService) : PageModel
{
    public ManagerDashboardDto DashboardData { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-30);

    [BindProperty(SupportsGet = true)]
    public DateTime EndDate { get; set; } = DateTime.Today;

    public async Task<IActionResult> OnGetAsync()
    {
        DashboardData = await dashboardService.GetDashboardDataAsync(StartDate, EndDate);
        return Page();
    }
}