using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Service.DTOs.Manager;

namespace Service.Manager;

public class ManagerDashboardService(AppDbContext context)
{
    public async Task<ManagerDashboardDto> GetDashboardDataAsync(DateTime startDate, DateTime endDate)
    {
        var result = new ManagerDashboardDto();
        var start = startDate.Date;
        var end = endDate.Date.AddDays(1).AddTicks(-1);

        var baseOrdersQuery = context.Orders.Where(o => o.OrderDate >= start && o.OrderDate <= end);
        var deliveredOrders = baseOrdersQuery.Where(o => o.OrderStatus == OrderStatus.Delivered);

        var totalRevenue = await deliveredOrders.SumAsync(o => (long?)o.TotalAmount) ?? 0;
        var totalOrders = await baseOrdersQuery.CountAsync();
        var totalDiscountCost = await deliveredOrders.SumAsync(o => (long?)(o.TotalAmountBeforeDiscount - o.TotalAmount)) ?? 0;

        var newCustomers = await context.Users
            .Where(u => u.CustomerData != null && u.CreatedAt >= start && u.CreatedAt <= end)
            .CountAsync();

        result.KPIs = new KpiStatsDto
        {
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            AverageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0,
            TotalDiscountCost = totalDiscountCost,
            NewCustomers = newCustomers
        };

        var rawRevenueData = await deliveredOrders
            .GroupBy(o => o.OrderDate.Date)
            .Select(g => new
            {
                Date = g.Key,
                TotalValue = g.Sum(o => o.TotalAmount)
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        var durationDays = (end - start).TotalDays;
        // Số ngày thực trong khoảng lọc (bao gồm cả ngày đầu và ngày cuối)
        var totalDaysInRange = (endDate.Date - startDate.Date).Days + 1;

        if (durationDays <= 31)
        {
            // FIX: Dùng DateOnly làm key để tránh lỗi DateTimeKind (Utc từ DB vs Unspecified từ request)
            var revenueByDay = rawRevenueData.ToDictionary(
                x => DateOnly.FromDateTime(x.Date),
                x => x.TotalValue
            );
            // FIX: Dùng totalDaysInRange thay vì (int)durationDays để không bị thiếu ngày cuối
            var allDays = Enumerable
                .Range(0, totalDaysInRange)
                .Select(i => DateOnly.FromDateTime(startDate.Date).AddDays(i));

            result.RevenueTrend = allDays
                .Select(day => new ChartItemDto
                {
                    Label = day.ToString("dd/MM/yyyy"),
                    Value = revenueByDay.TryGetValue(day, out var val) ? val : 0
                })
                .ToList();
        }
        else if (durationDays <= 90)
        {
            // Zero-fill theo Tuần: tạo đủ tất cả các tuần trong khoảng lọc
            var revenueByWeek = rawRevenueData
                .GroupBy(x => new
                {
                    Year = System.Globalization.ISOWeek.GetYear(x.Date),
                    Week = System.Globalization.ISOWeek.GetWeekOfYear(x.Date)
                })
                .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalValue));

            var allWeeks = Enumerable
                .Range(0, totalDaysInRange)
                .Select(i => startDate.Date.AddDays(i))
                .Select(d => new
                {
                    Year = System.Globalization.ISOWeek.GetYear(d),
                    Week = System.Globalization.ISOWeek.GetWeekOfYear(d)
                })
                .Distinct();

            result.RevenueTrend = allWeeks
                .Select(w => new ChartItemDto
                {
                    Label = $"Tuần {w.Week:D2}/{w.Year}",
                    Value = revenueByWeek.TryGetValue(w, out var val) ? val : 0
                })
                .ToList();
        }
        else if (durationDays <= 365)
        {
            // Zero-fill theo Tháng: tạo đủ tất cả các tháng trong khoảng lọc
            var revenueByMonth = rawRevenueData
                .GroupBy(x => new { x.Date.Year, x.Date.Month })
                .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalValue));

            var allMonths = Enumerable
                .Range(0, totalDaysInRange)
                .Select(i => startDate.Date.AddDays(i))
                .Select(d => new { d.Year, d.Month })
                .Distinct();

            result.RevenueTrend = allMonths
                .Select(m => new ChartItemDto
                {
                    Label = $"{m.Month:D2}/{m.Year}",
                    Value = revenueByMonth.TryGetValue(m, out var val) ? val : 0
                })
                .ToList();
        }
        else
        {
            // Zero-fill theo Năm: tạo đủ tất cả các năm trong khoảng lọc
            var revenueByYear = rawRevenueData
                .GroupBy(x => x.Date.Year)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalValue));

            var allYears = Enumerable
                .Range(startDate.Year, endDate.Year - startDate.Year + 1);

            result.RevenueTrend = allYears
                .Select(y => new ChartItemDto
                {
                    Label = y.ToString(),
                    Value = revenueByYear.TryGetValue(y, out var val) ? val : 0
                })
                .ToList();
        }

        var rawOrderStatus = await baseOrdersQuery
            .GroupBy(o => o.OrderStatus)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        result.OrderStatusDistribution = rawOrderStatus
            .Select(x => new ChartItemDto
            {
                Label = x.Status.ToString(),
                Value = x.Count
            })
            .ToList();

        var rawPaymentMethod = await baseOrdersQuery
            .GroupBy(o => o.PaymentMethod)
            .Select(g => new
            {
                Method = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        result.PaymentMethodDistribution = rawPaymentMethod
            .Select(x => new ChartItemDto
            {
                Label = x.Method.ToString(),
                Value = x.Count
            })
            .ToList();

        result.TopProducts = await context.OrderItems
            .Include(oi => oi.Order)
            .Where(oi => oi.Order!.OrderDate >= start && oi.Order.OrderDate <= end && oi.Order.OrderStatus == OrderStatus.Delivered)
            .GroupBy(oi => new { oi.ProductId, oi.ProductSnapshot.Name })
            .Select(g => new TopProductDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name,
                TotalSold = g.Sum(oi => oi.Quantity),
                Revenue = g.Sum(oi => oi.Quantity * oi.ProductSnapshot.UnitPrice)
            })
            .OrderByDescending(x => x.TotalSold)
            .Take(10)
            .ToListAsync();

        result.LowStockProducts = await context.Products
            .Where(p => p.IsActive && p.Quantity <= 10)
            .OrderBy(p => p.Quantity)
            .Select(p => new LowStockProductDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Quantity = p.Quantity
            })
            .ToListAsync();

        var revenueByCategory = await context.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Categories)
            .Where(oi => oi.Order!.OrderDate >= start && oi.Order.OrderDate <= end && oi.Order.OrderStatus == OrderStatus.Delivered)
            .SelectMany(oi => oi.Product!.Categories!.DefaultIfEmpty(), (oi, cat) => new { oi, cat })
            .Where(x => x.cat != null)
            .GroupBy(x => x.cat!.Name)
            .Select(g => new ChartItemDto
            {
                Label = g.Key,
                Value = g.Sum(x => x.oi.Quantity * x.oi.ProductSnapshot.UnitPrice)
            })
            .OrderByDescending(c => c.Value)
            .Take(5)
            .ToListAsync();

        result.RevenueByCategory = revenueByCategory;

        var allCoupons = await context.CustomerCoupons
            .Include(cc => cc.Coupon)
            .Where(cc => cc.Coupon != null)
            .GroupBy(cc => new { cc.CouponId, cc.Coupon!.Description })
            .Select(g => new CouponUsageDto
            {
                CouponCode = g.Key.Description,
                TotalIssued = g.Count(),
                TotalUsed = g.Count(cc => cc.IsUsed)
            })
            .ToListAsync();

        result.CouponUsage = allCoupons
            .OrderByDescending(c => c.UsageRate)
            .ThenByDescending(c => c.TotalIssued)
            .Take(5)
            .ToList();

        var totalPointsEarned = await baseOrdersQuery.SumAsync(o => (long?)o.LoyaltyPointsEarned) ?? 0;
        var totalPointsRetained = await context.CustomerData.SumAsync(c => (long?)c.LoyaltyPoints) ?? 0;

        result.LoyaltyStats = new LoyaltyStatsDto
        {
            TotalPointsIssued = (int)totalPointsEarned,
            TotalPointsRetained = (int)totalPointsRetained
        };

        var reviewsInPeriod = context.ProductReviews.Where(r => r.CreatedAt >= start && r.CreatedAt <= end);
        var totalReviews = await reviewsInPeriod.CountAsync();

        result.SatisfactionStats.TotalReviews = totalReviews;

        if (totalReviews > 0)
        {
            result.SatisfactionStats.AverageRating = await reviewsInPeriod.AverageAsync(r => r.Rating);

            var rawFeedback = await reviewsInPeriod
                .GroupBy(r => r.CommentClassification)
                .Select(g => new
                {
                    Classification = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            result.SatisfactionStats.FeedbackClassification = rawFeedback
                .Select(x => new ChartItemDto
                {
                    Label = x.Classification.ToString(),
                    Value = x.Count
                })
                .ToList();
        }

        return result;
    }
}