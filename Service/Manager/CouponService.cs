using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Models.Coupons;
using Service.DTOs.Manager;

namespace Service.Manager;

public class CouponService(AppDbContext context,CouponMapper mapper)
{
    public async Task<List<Coupon>> GetAllCouponsAsync()
    {
        return await context.Coupons
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CouponUpdateDto?> GetCouponByIdAsync(int id)
    {
        var query = await context.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (query == null) return null;
        return mapper.ToCouponUpdateDto(query);
    }

    public async Task<Coupon?> UpdateCouponAsync(int id, CouponUpdateDto input)
    {
        var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.Id == id);
        if (coupon == null)
        {
            return null;
        }
        if (input is { DiscountType: DiscountType.Percentage, DiscountValue: > 100 })
        {
            return null;
        }
        
        coupon.Description = input.Description;
        coupon.DiscountType = input.DiscountType;
        coupon.DiscountValue = input.DiscountValue;
        coupon.MaxDiscountAmount = input.MaxDiscountAmount;
        coupon.MinOrderAmount = input.MinOrderAmount;
        coupon.LoyaltyPointsCost = input.LoyaltyPointsCost;
        coupon.IsActive = input.IsActive;

        await context.SaveChangesAsync();
        return coupon;
    }
}