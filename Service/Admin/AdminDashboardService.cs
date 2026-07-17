using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Service.DTOs.Admin;

namespace Service.Admin;

public class AdminDashboardService(AppDbContext context)
{
    public async Task<AdminDashboardDto> GetDashboardDataAsync()
    {
        var result = new AdminDashboardDto();
        var now = DateTime.UtcNow;
        var today = now.Date;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);

        result.KPIs = new AdminKpiDto
        {
            TotalUsers = await context.Users.CountAsync(),
            NewUsersToday = await context.Users.CountAsync(u => u.CreatedAt >= today),
            NewUsersThisWeek = await context.Users.CountAsync(u => u.CreatedAt >= startOfWeek)
        };

        var roleDistribution = await context.Roles
            .Select(r => new AdminChartItemDto
            {
                Label = r.Name ?? "Unknown",
                Value = context.UserRoles.Count(ur => ur.RoleId == r.Id)
            })
            .ToListAsync();
        result.RoleDistribution = roleDistribution;

        var anomalyProducts = await context.Products
            .Where(p => p.Quantity < 0 || p.HeldQuantity < 0)
            .Select(p => new InventoryAnomalyDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Quantity = p.Quantity,
                HeldQuantity = p.HeldQuantity,
                IssueDescription = p.Quantity < 0 ? "Tồn kho khả dụng bị âm" : "Tồn kho tạm giữ bị âm"
            })
            .ToListAsync();
        result.InventoryAnomalies = anomalyProducts;

        var stuckQrOrders = await context.Orders
            .Include(o => o.QrCodePaymentData)
            .Where(o => o.PaymentMethod == PaymentMethod.QRCode
                     && o.OrderStatus == OrderStatus.PendingPayment
                     && o.QrCodePaymentData != null
                     && o.QrCodePaymentData.ExpirationDate < now.AddMinutes(-5))
            .Select(o => new StuckQrOrderDto
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                ExpirationDate = o.QrCodePaymentData!.ExpirationDate
            })
            .ToListAsync();
        result.StuckQrOrders = stuckQrOrders;

        var stuckCoupons = await context.CustomerCoupons
            .Where(cc => cc.ExpiryDate != null && cc.ExpiryDate < now.AddDays(-1))
            .Select(cc => new StuckCouponDto
            {
                CustomerCouponId = cc.Id,
                CustomerId = cc.CustomerId,
                ExpiryDate = cc.ExpiryDate!.Value
            })
            .ToListAsync();
        result.StuckCoupons = stuckCoupons;

        return result;
    }
}