using Repository.Constants;
using Service.DTOs.Customer.Order;

namespace Service.DTOs.Customer;

public class CustomerDashboardDto
{
    public CustomerKpiDto KPIs { get; set; } = new();
    public List<MonthlySpentDto> SpentChart { get; set; } = []; // 6 tháng gần nhất
    public List<OrderSummaryDto> RecentOrders { get; set; } = [];
}

public class CustomerKpiDto
{
    public int TotalOrders { get; set; }
    public int ActiveOrders { get; set; }       // PendingConfirmation + Processing + Shipping
    public long TotalSpent { get; set; }        // Tổng chi tiêu (Delivered orders)
    public long LoyaltyPoints { get; set; }
    public int AvailableCoupons { get; set; }
}

public class MonthlySpentDto
{
    public string Label { get; set; } = string.Empty;  // "Th.1", "Th.2", ...
    public long TotalSpent { get; set; }
    public int OrderCount { get; set; }
}
