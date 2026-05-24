using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Manager;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class CouponMapper
{
    public static partial IQueryable<CouponDto> ProjectToCouponDto(this IQueryable<Repository.Models.Coupons.Coupon> coupon);
    public static partial IQueryable<CouponUpdateDto> ProjectToCouponUpdateDto(this IQueryable<Repository.Models.Coupons.Coupon> coupon);
}