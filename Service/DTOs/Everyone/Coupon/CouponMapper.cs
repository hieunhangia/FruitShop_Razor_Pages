using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Everyone.Coupon;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class CouponMapper
{
    public static partial IQueryable<CouponShopDto> ProjectToCouponShopDto(
        this IQueryable<Repository.Models.Coupons.Coupon> coupons);
}