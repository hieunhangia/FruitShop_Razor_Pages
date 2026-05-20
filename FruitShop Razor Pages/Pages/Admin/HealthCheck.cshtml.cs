using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.Admin.HealthCheck;
using Service.DTOs.Admin.HealthCheck;

namespace FruitShop_Razor_Pages.Pages.Admin;

[Authorize(Roles = Role.Admin)]
public class HealthCheckModel(HealthCheckService healthCheckService) : PageModel
{
    public HealthCheckDto? HealthCheck { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            HealthCheck = await healthCheckService.HealthCheckAsync(baseUrl, Request.Headers.Cookie.ToString());
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
        }

        return Page();
    }
}