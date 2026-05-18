using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Service.DTOs.Admin.HealthCheck;

public class HealthCheckDto
{
    public required HealthStatus Status { get; set; }
    public required List<HealthCheckItemDto> Items { get; set; }
}