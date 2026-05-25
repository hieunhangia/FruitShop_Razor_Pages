namespace Service.DTOs.SalesStaff;

public class DashboardStatsDto
{
    public int TotalProducts { get; set; }
    public int ActiveProducts { get; set; }
    public int TotalCategories { get; set; }
    public int ActiveCategories { get; set; }
    public int TotalProductUnits { get; set; }
}