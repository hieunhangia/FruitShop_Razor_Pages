using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs.Customer.Coupon;

namespace Service.Customer;

public class CouponService(AppDbContext context, CouponMapper mapper)
{
    public async Task<List<CouponInCheckoutPageDto>>
        GetAvailableCouponsForOrderAsync(int customerId, long totalAmount) => mapper.ToAvailableCouponForOrderDtoList(
        await context.CustomerCoupons.AsNoTracking()
            .Include(c => c.Coupon)
            .Where(cc =>
                cc.CustomerId == customerId && cc.ExpiryDate > DateTime.UtcNow && !cc.IsUsed &&
                (!cc.Coupon!.MinOrderAmount.HasValue || cc.Coupon.MinOrderAmount.Value <= totalAmount))
            .ToListAsync());
}