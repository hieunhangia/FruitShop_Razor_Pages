using Riok.Mapperly.Abstractions;
using Coupons = Repository.Models.Coupons;

namespace Service.DTOs.Customer.Coupon;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class CouponMapper
{
    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.Coupon)}.{nameof(Coupons.Coupon.Id)}",
        nameof(CouponViewDto.Id))]
    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.Coupon)}.{nameof(Coupons.Coupon.Description)}",
        nameof(CouponViewDto.Description))]
    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.Coupon)}.{nameof(Coupons.Coupon.DiscountValue)}",
        nameof(CouponViewDto.DiscountValue))]
    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.Coupon)}.{nameof(Coupons.Coupon.DiscountType)}",
        nameof(CouponViewDto.DiscountType))]
    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.Coupon)}.{nameof(Coupons.Coupon.MaxDiscountAmount)}",
        nameof(CouponViewDto.MaxDiscountAmount))]
    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.Coupon)}.{nameof(Coupons.Coupon.LoyaltyPointsCost)}",
        nameof(CouponViewDto.LoyaltyPointsCost))]
    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.IsUsed)}",
        nameof(CouponViewDto.IsUsed))]
    public static partial CouponViewDto ToCouponViewDto(Coupons.CustomerCoupon customerCoupon);

    public static partial IQueryable<CouponViewDto> ProjectToCouponViewDto(
        this IQueryable<Coupons.CustomerCoupon> customerCoupons);

    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.Coupon)}.{nameof(Coupons.Coupon.Description)}",
        nameof(CouponInCheckoutPageDto.Description))]
    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.Coupon)}.{nameof(Coupons.Coupon.DiscountValue)}",
        nameof(CouponInCheckoutPageDto.DiscountValue))]
    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.Coupon)}.{nameof(Coupons.Coupon.DiscountType)}",
        nameof(CouponInCheckoutPageDto.DiscountType))]
    [MapProperty(
        $"{nameof(Coupons.CustomerCoupon.Coupon)}.{nameof(Coupons.Coupon.MaxDiscountAmount)}",
        nameof(CouponInCheckoutPageDto.MaxDiscountAmount))]
    private static partial CouponInCheckoutPageDto ToAvailableCouponForOrderDto(Coupons.CustomerCoupon customerCoupon);

    public static partial IQueryable<CouponInCheckoutPageDto> ProjectToAvailableCouponForOrderDto(
        this IQueryable<Coupons.CustomerCoupon> customerCoupons);
}