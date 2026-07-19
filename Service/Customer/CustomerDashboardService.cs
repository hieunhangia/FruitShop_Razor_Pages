using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Service.DTOs.Customer;
using Service.DTOs.Customer.Order;

namespace Service.Customer;

public class CustomerDashboardService(AppDbContext context)
{
    public async Task<CustomerDashboardDto> GetDashboardDataAsync(int customerId)
    {
        var result = new CustomerDashboardDto();
        var now = DateTime.UtcNow;

        // === KPIs ===
        var loyaltyPoints = await context.CustomerData
            .Where(cd => cd.CustomerId == customerId)
            .Select(cd => cd.LoyaltyPoints)
            .FirstOrDefaultAsync();

        var availableCoupons = await context.CustomerCoupons
            .CountAsync(cc => cc.CustomerId == customerId
                           && !cc.IsUsed
                           && (cc.ExpiryDate == null || cc.ExpiryDate > now));

        var activeStatuses = new[]
        {
            OrderStatus.PendingConfirmation,
            OrderStatus.PendingPayment,
            OrderStatus.Processing,
            OrderStatus.Shipping
        };

        var allOrders = await context.Orders
            .Where(o => o.CustomerId == customerId)
            .Select(o => new { o.OrderDate, o.TotalAmount, o.OrderStatus })
            .ToListAsync();

        result.KPIs = new CustomerKpiDto
        {
            TotalOrders = allOrders.Count,
            ActiveOrders = allOrders.Count(o => activeStatuses.Contains(o.OrderStatus)),
            TotalSpent = allOrders
                .Where(o => o.OrderStatus == OrderStatus.Delivered)
                .Sum(o => o.TotalAmount),
            LoyaltyPoints = loyaltyPoints,
            AvailableCoupons = availableCoupons
        };

        // === Monthly Spent Chart (last 6 months) ===
        result.SpentChart = Enumerable.Range(0, 6)
            .Select(i =>
            {
                var targetMonth = now.AddMonths(-5 + i);
                var monthOrders = allOrders
                    .Where(o => o.OrderStatus == OrderStatus.Delivered
                             && o.OrderDate.Year == targetMonth.Year
                             && o.OrderDate.Month == targetMonth.Month)
                    .ToList();
                return new MonthlySpentDto
                {
                    Label = $"Th.{targetMonth.Month}",
                    TotalSpent = monthOrders.Sum(o => o.TotalAmount),
                    OrderCount = monthOrders.Count
                };
            })
            .ToList();

        // === Recent 5 Orders ===
        result.RecentOrders = await context.Orders
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .Take(5)
            .Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus,
                PaymentMethod = o.PaymentMethod,
                TotalAmount = o.TotalAmount
            })
            .ToListAsync();

        return result;
    }
}
