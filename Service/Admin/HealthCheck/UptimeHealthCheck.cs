using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Service.Admin.HealthCheck;

public class UptimeHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var startTime = Process.GetCurrentProcess().StartTime.ToUniversalTime();
        var uptime = DateTime.UtcNow - startTime;

        var data = new Dictionary<string, object>
        {
            { "Khởi chạy lúc", startTime.ToLocalTime().ToString("HH:mm:ss dd/MM/yyyy") },
            { "Thời gian hoạt động", TimeSpanToString(uptime) }
        };
        return Task.FromResult(HealthCheckResult.Healthy("Ứng dụng đang hoạt động bình thường.", data));
    }

    private static string TimeSpanToString(TimeSpan ts)
    {
        if (ts == TimeSpan.Zero) return "0 giây";
        var sb = new StringBuilder(40);
        var days = ts.Days;
        var hours = ts.Hours;
        var minutes = ts.Minutes;
        var seconds = ts.Seconds;

        if (days > 0)
        {
            sb.Append(days).Append(" ngày");
        }

        if (hours > 0)
        {
            if (sb.Length > 0) sb.Append(", ");
            sb.Append(hours).Append(" giờ");
        }

        if (minutes > 0)
        {
            if (sb.Length > 0) sb.Append(", ");
            sb.Append(minutes).Append(" phút");
        }

        if (seconds > 0)
        {
            if (sb.Length > 0) sb.Append(", ");
            sb.Append(seconds).Append(" giây");
        }

        return sb.ToString();
    }
}