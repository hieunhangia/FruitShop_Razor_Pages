using Microsoft.Extensions.Diagnostics.HealthChecks;
using Repository;

namespace Service.Admin.HealthCheck;

public class PayOsHealthCheck(IHttpClientFactory httpClientFactory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var response =
                await httpClient.GetAsync(BusinessRuleConstants.AdminRoute.PayOsApiHealthCheck, cancellationToken);
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("PayOS API đang hoạt động bình thường.")
                : HealthCheckResult.Unhealthy($"PayOS API không phản hồi đúng: Status code {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"PayOS API đang gặp sự cố: {ex.Message}");
        }
    }
}