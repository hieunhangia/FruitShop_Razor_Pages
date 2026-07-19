using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Service.DTOs.Shipper;

namespace Service.Shipper;

public class ShipperDashboardService(AppDbContext context)
{
    public async Task<ShipperDashboardDto> GetDashboardDataAsync(int shipperId)
    {
        var result = new ShipperDashboardDto();
        var now = DateTime.UtcNow;
        var todayUtc = now.Date;
        var startOfMonthUtc = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var sevenDaysAgo = todayUtc.AddDays(-6);

        // === KPIs ===
        var deliveredOrders = await context.Orders
            .Where(o => o.ShipperId == shipperId && o.OrderStatus == OrderStatus.Delivered)
            .Select(o => new { o.OrderDate, o.TotalAmount })
            .ToListAsync();

        result.KPIs = new ShipperKpiDto
        {
            TotalDeliveredToday = deliveredOrders
                .Count(o => o.OrderDate.Date == todayUtc),

            TotalDeliveredThisMonth = deliveredOrders
                .Count(o => o.OrderDate >= startOfMonthUtc),

            ActiveOrders = await context.Orders
                .CountAsync(o => o.ShipperId == shipperId && o.OrderStatus == OrderStatus.Shipping),

            TotalRevenueDeliveredThisMonth = deliveredOrders
                .Where(o => o.OrderDate >= startOfMonthUtc)
                .Sum(o => o.TotalAmount)
        };

        // === Delivery Chart (last 7 days) ===
        var recentDelivered = deliveredOrders
            .Where(o => o.OrderDate.Date >= sevenDaysAgo)
            .ToList();

        result.DeliveryChart = Enumerable.Range(0, 7)
            .Select(i =>
            {
                var day = sevenDaysAgo.AddDays(i);
                return new DailyDeliveryDto
                {
                    Label = day.ToString("dd/MM"),
                    DeliveredCount = recentDelivered.Count(o => o.OrderDate.Date == day)
                };
            })
            .ToList();

        return result;
    }
}
