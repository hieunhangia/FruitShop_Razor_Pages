using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.Manager;

public class ManagerDashboardDto
{
    public KpiStatsDto KPIs { get; set; } = new();
    public List<ChartItemDto> RevenueTrend { get; set; } = [];
    public List<ChartItemDto> OrderStatusDistribution { get; set; } = [];
    public List<ChartItemDto> PaymentMethodDistribution { get; set; } = [];
    public List<TopProductDto> TopProducts { get; set; } = [];
    public List<LowStockProductDto> LowStockProducts { get; set; } = [];
    public List<ChartItemDto> RevenueByCategory { get; set; } = [];
    public List<CouponUsageDto> CouponUsage { get; set; } = [];
    public LoyaltyStatsDto LoyaltyStats { get; set; } = new();
    public CustomerSatisfactionDto SatisfactionStats { get; set; } = new();
}

public class KpiStatsDto
{
    public long TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public long AverageOrderValue { get; set; }
    public long TotalDiscountCost { get; set; }
    public int NewCustomers { get; set; }
}

public class ChartItemDto
{
    public required string Label { get; set; }
    public decimal Value { get; set; }
}

public class TopProductDto
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public int TotalSold { get; set; }
    public long Revenue { get; set; }
}

public class LowStockProductDto
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public int Quantity { get; set; }
}

public class CouponUsageDto
{
    public required string CouponCode { get; set; }
    public int TotalIssued { get; set; }
    public int TotalUsed { get; set; }
    public decimal UsageRate => TotalIssued == 0 ? 0 : (decimal)TotalUsed / TotalIssued * 100;
}

public class LoyaltyStatsDto
{
    public int TotalPointsIssued { get; set; }
    public int TotalPointsRetained { get; set; }
}

public class CustomerSatisfactionDto
{
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public List<ChartItemDto> FeedbackClassification { get; set; } = [];
}