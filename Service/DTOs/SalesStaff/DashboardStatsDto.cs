namespace Service.DTOs.SalesStaff;

public class DashboardStatsDto
{
    public int TotalProducts { get; set; }
    public int ActiveProducts { get; set; }
    public int TotalCategories { get; set; }
    public int ActiveCategories { get; set; }
    public int TotalProductUnits { get; set; }
}

// ===== Sales Dashboard Analytics =====

public class SalesDashboardDto
{
    public SalesKpiDto KPIs { get; set; } = new();
    public List<DailyRevenueDto> RevenueChart { get; set; } = [];
    public List<TopProductDto> TopProducts { get; set; } = [];
    public List<DashboardChartItemDto> OrderStatusChart { get; set; } = [];
}

public class SalesKpiDto
{
    public int TotalOrdersToday { get; set; }
    public int PendingOrders { get; set; }
    public long RevenueThisMonth { get; set; }
    public int LowStockProducts { get; set; }
}

public class DailyRevenueDto
{
    public string Label { get; set; } = string.Empty; // "dd/MM"
    public long Revenue { get; set; }
    public int OrderCount { get; set; }
}

public class TopProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int TotalSold { get; set; }
    public long TotalRevenue { get; set; }
}

public class DashboardChartItemDto
{
    public string Label { get; set; } = string.Empty;
    public int Value { get; set; }
}