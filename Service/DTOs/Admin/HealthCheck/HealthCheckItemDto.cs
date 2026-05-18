using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Service.DTOs.Admin.HealthCheck;

public class HealthCheckItemDto
{
    public required string Name { get; set; }
    public required HealthStatus Status { get; set; }
    public string? Description { get; set; }
    public required IReadOnlyDictionary<string, object> Data { get; set; }
    public string? ExceptionStackTrace { get; set; }
}