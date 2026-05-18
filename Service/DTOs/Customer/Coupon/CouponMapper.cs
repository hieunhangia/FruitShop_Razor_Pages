using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Coupon;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CouponMapper
{
    [MapProperty(
        $"{nameof(Repository.Models.Coupons.CustomerCoupon.Coupon)}.{nameof(Repository.Models.Coupons.Coupon.Description)}",
        nameof(CouponInCheckoutPageDto.Description))]
    [MapProperty(
        $"{nameof(Repository.Models.Coupons.CustomerCoupon.Coupon)}.{nameof(Repository.Models.Coupons.Coupon.DiscountValue)}",
        nameof(CouponInCheckoutPageDto.DiscountValue))]
    [MapProperty(
        $"{nameof(Repository.Models.Coupons.CustomerCoupon.Coupon)}.{nameof(Repository.Models.Coupons.Coupon.DiscountType)}",
        nameof(CouponInCheckoutPageDto.DiscountType))]
    [MapProperty(
        $"{nameof(Repository.Models.Coupons.CustomerCoupon.Coupon)}.{nameof(Repository.Models.Coupons.Coupon.MaxDiscountAmount)}",
        nameof(CouponInCheckoutPageDto.MaxDiscountAmount))]
    public partial CouponInCheckoutPageDto
        ToAvailableCouponForOrderDto(Repository.Models.Coupons.CustomerCoupon customerCoupon);

    public partial List<CouponInCheckoutPageDto> ToAvailableCouponForOrderDtoList(
        List<Repository.Models.Coupons.CustomerCoupon> customerCoupons);
}