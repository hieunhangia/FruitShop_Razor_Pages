using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.Admin.HealthCheck;
using Service.DTOs.Admin.HealthCheck;

namespace FruitShop_Razor_Pages.Pages.Admin;

[Authorize(Roles = Role.Admin)]
public class DashboardModel(HealthCheckService healthCheckService) : PageModel
{
    public HealthCheckDto? HealthCheck { get; set; }

    public async Task OnGetAsync()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        HealthCheck = await healthCheckService.HealthCheckAsync(baseUrl, Request.Headers.Cookie.ToString());
    }
}