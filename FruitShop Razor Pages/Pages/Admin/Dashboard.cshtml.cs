using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Repository.Constants;
using Service.Admin;
using HealthCheckService = Service.Admin.HealthCheck.HealthCheckService;

namespace FruitShop_Razor_Pages.Pages.Admin;

[Authorize(Roles = Role.Admin)]
public class DashboardModel(HealthCheckService healthCheckService, AccountService accountService) : PageModel
{
    public HealthStatus HealthStatus { get; set; } = HealthStatus.Unhealthy;
    public int UserCount { get; set; }
    public int SalesStaffCount { get; set; }
    public int ShipperCount { get; set; }
    public int CustomerSupportCount { get; set; }
    public int CustomerCount { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            HealthStatus = (await healthCheckService.HealthCheckAsync(baseUrl, Request.Headers.Cookie.ToString()))
                .Status;
            UserCount = await accountService.CountUserAsync();
            SalesStaffCount = await accountService.CountSalesStaffAsync();
            ShipperCount = await accountService.CountShipperAsync();
            CustomerSupportCount = await accountService.CountCustomerSupportAsync();
            CustomerCount = await accountService.CountCustomerAsync();
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
        }

        return Page();
    }
}