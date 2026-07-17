namespace Service.DTOs.Admin;

public class AdminDashboardDto
{
    public AdminKpiDto KPIs { get; set; } = new();
    public List<AdminChartItemDto> RoleDistribution { get; set; } = [];
    public List<InventoryAnomalyDto> InventoryAnomalies { get; set; } = [];
    public List<StuckQrOrderDto> StuckQrOrders { get; set; } = [];
    public List<StuckCouponDto> StuckCoupons { get; set; } = [];
}

public class AdminKpiDto
{
    public int TotalUsers { get; set; }
    public int NewUsersToday { get; set; }
    public int NewUsersThisWeek { get; set; }
}

public class AdminChartItemDto
{
    public required string Label { get; set; }
    public int Value { get; set; }
}

public class InventoryAnomalyDto
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public int Quantity { get; set; }
    public int HeldQuantity { get; set; }
    public required string IssueDescription { get; set; }
}

public class StuckQrOrderDto
{
    public long OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ExpirationDate { get; set; }
}

public class StuckCouponDto
{
    public int CustomerCouponId { get; set; }
    public int CustomerId { get; set; }
    public DateTime ExpiryDate { get; set; }
}