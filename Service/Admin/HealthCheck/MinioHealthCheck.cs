using Microsoft.Extensions.Diagnostics.HealthChecks;
using Repository;

namespace Service.Admin.HealthCheck;

public class MinioHealthCheck(IHttpClientFactory httpClientFactory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient();
            var response =
                await httpClient.GetAsync(BusinessRuleConstants.HealthCheck.MinioApiHealthCheck, cancellationToken);
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("Minio đang hoạt động bình thường.")
                : HealthCheckResult.Unhealthy($"Minio không phản hồi đúng: Status code {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Minio đang gặp sự cố: {ex.Message}", ex);
        }
    }
}