using System.Net.Http.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Repository;
using Service.DTOs.Admin.HealthCheck;

namespace Service.Admin.HealthCheck;

public class HealthCheckService(IHttpClientFactory httpClientFactory, ILogger<HealthCheckService> logger)
{
    public async Task<HealthCheckDto> HealthCheckAsync(string baseUrl, string? cookieHeader)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            if (!string.IsNullOrEmpty(cookieHeader))
            {
                httpClient.DefaultRequestHeaders.Add("Cookie", cookieHeader);
            }

            var response = await httpClient.GetAsync($"{baseUrl}{BusinessRuleConstants.HealthCheck.HealthCheckApi}");
            return await response.Content.ReadFromJsonAsync<HealthCheckDto>()
                   ?? throw new Exception("Response was successful but JSON parsing returned null.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while performing health check");
            return new HealthCheckDto
            {
                Status = HealthStatus.Unhealthy,
                Items = []
            };
        }
    }
}