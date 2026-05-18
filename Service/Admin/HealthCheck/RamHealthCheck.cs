using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Service.Admin.HealthCheck;

public class RamHealthCheck(long maxRamUsageMB) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var totalRamMB = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / 1024 / 1024;
        var ramUsageMB = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;
        var data = new Dictionary<string, object>
        {
            { "Tổng RAM hệ thống (MB)", totalRamMB },
            { "RAM được ứng dụng sử dụng (MB)", ramUsageMB }
        };
        if (ramUsageMB > maxRamUsageMB)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(
                $"Hệ thống đang sử dụng lượng RAM vượt quá mức cảnh báo ({maxRamUsageMB} MB)", data: data));
        }

        return Task.FromResult(
            HealthCheckResult.Healthy("Ứng dụng đang sử dụng lượng RAM ổn định", data: data));
    }
}