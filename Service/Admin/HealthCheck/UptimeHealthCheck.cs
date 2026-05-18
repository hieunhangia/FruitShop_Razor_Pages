using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Service.Admin.HealthCheck;

public class UptimeHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var startTime = Process.GetCurrentProcess().StartTime;
        var uptime = DateTime.Now - startTime;

        var data = new Dictionary<string, object>
        {
            { "Khởi chạy lúc", startTime.ToString("HH:mm:ss dd/MM/yyyy") },
            { "Tổng thời gian hoạt động (s)", Math.Round(uptime.TotalSeconds) }
        };
        return Task.FromResult(HealthCheckResult.Healthy("Ứng dụng đang hoạt động bình thường.", data));
    }
}