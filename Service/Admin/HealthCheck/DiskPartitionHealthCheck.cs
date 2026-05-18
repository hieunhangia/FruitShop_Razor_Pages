using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Service.Admin.HealthCheck;

public class DiskPartitionHealthCheck(int minimumFreeSpaceUnhealthyGB) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        try
        {
            var appDirectory = AppContext.BaseDirectory;
            var driveRoot = Path.GetPathRoot(appDirectory);

            if (string.IsNullOrEmpty(driveRoot))
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Không thể xác định phân vùng chứa ứng dụng."));
            }

            var driveInfo = new DriveInfo(driveRoot);
            var freeSpaceGb = driveInfo.AvailableFreeSpace / 1024.0 / 1024.0 / 1024.0;
            var totalSizeGb = driveInfo.TotalSize / 1024.0 / 1024.0 / 1024.0;
            var data = new Dictionary<string, object>
            {
                { "Tổng dung lượng (GB)", Math.Round(totalSizeGb, 2) },
                { "Dung lượng trống (GB)", Math.Round(freeSpaceGb, 2) }
            };
            if (freeSpaceGb < minimumFreeSpaceUnhealthyGB)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(
                    $"Phân vùng đang có dung lượng trống ít hơn mức cảnh báo ({minimumFreeSpaceUnhealthyGB} GB)",
                    data: data));
            }

            return Task.FromResult(HealthCheckResult.Healthy("Phân vùng đang có dung lượng ổn định.", data: data));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy($"Lỗi khi kiểm tra dung lượng phân vùng: {ex.Message}",
                ex));
        }
    }
}