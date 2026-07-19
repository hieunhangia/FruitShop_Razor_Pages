using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Service.DTOs.SalesStaff;

namespace Service.SalesStaff;

public class SalesDashboardService(AppDbContext context)
{
    public async Task<SalesDashboardDto> GetDashboardDataAsync()
    {
        var result = new SalesDashboardDto();
        var now = DateTime.UtcNow;
        var todayUtc = now.Date;
        var startOfMonthUtc = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var lowStockThreshold = 10;

        // === KPIs ===
        result.KPIs = new SalesKpiDto
        {
            TotalOrdersToday = await context.Orders
                .CountAsync(o => o.OrderDate >= todayUtc && o.OrderDate < todayUtc.AddDays(1)),

            PendingOrders = await context.Orders
                .CountAsync(o => o.OrderStatus == OrderStatus.PendingConfirmation
                              || o.OrderStatus == OrderStatus.Processing),

            RevenueThisMonth = await context.Orders
                .Where(o => o.OrderStatus == OrderStatus.Delivered
                         && o.OrderDate >= startOfMonthUtc)
                .SumAsync(o => (long?)o.TotalAmount) ?? 0L,

            LowStockProducts = await context.Products
                .CountAsync(p => p.IsActive && p.Quantity <= lowStockThreshold)
        };

        // === Revenue Chart (last 30 days) ===
        var thirtyDaysAgo = todayUtc.AddDays(-29);
        var deliveredOrders = await context.Orders
            .Where(o => o.OrderStatus == OrderStatus.Delivered
                     && o.OrderDate >= thirtyDaysAgo)
            .Select(o => new { o.OrderDate, o.TotalAmount })
            .ToListAsync();

        result.RevenueChart = Enumerable.Range(0, 30)
            .Select(i =>
            {
                var day = thirtyDaysAgo.AddDays(i);
                var dayOrders = deliveredOrders
                    .Where(o => o.OrderDate.Date == day.Date)
                    .ToList();
                return new DailyRevenueDto
                {
                    Label = day.ToString("dd/MM"),
                    Revenue = dayOrders.Sum(o => o.TotalAmount),
                    OrderCount = dayOrders.Count
                };
            })
            .ToList();

        // === Top 5 Products (last 30 days) ===
        result.TopProducts = await context.OrderItems
            .Where(oi => oi.Order!.OrderStatus == OrderStatus.Delivered
                      && oi.Order.OrderDate >= thirtyDaysAgo)
            .GroupBy(oi => new { oi.ProductId, oi.ProductSnapshot!.Name })
            .Select(g => new TopProductDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                TotalSold = g.Sum(oi => oi.Quantity),
                TotalRevenue = g.Sum(oi => (long)oi.Quantity * oi.ProductSnapshot!.UnitPrice)
            })
            .OrderByDescending(t => t.TotalSold)
            .Take(5)
            .ToListAsync();

        // === Order Status Distribution (all time) ===
        var statusGroups = await context.Orders
            .GroupBy(o => o.OrderStatus)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        result.OrderStatusChart = statusGroups
            .Select(g => new DashboardChartItemDto
            {
                Label = g.Status switch
                {
                    OrderStatus.PendingConfirmation => "Chờ xác nhận",
                    OrderStatus.PendingPayment => "Chờ thanh toán",
                    OrderStatus.Processing => "Đang xử lý",
                    OrderStatus.Shipping => "Đang giao",
                    OrderStatus.Delivered => "Đã giao",
                    OrderStatus.Cancelled => "Đã huỷ",
                    _ => g.Status.ToString()
                },
                Value = g.Count
            })
            .ToList();

        return result;
    }
}
