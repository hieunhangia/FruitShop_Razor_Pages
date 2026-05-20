using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Coupon;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CouponMapper
{
    [MapProperty(
        nameof(Repository.Models.Coupons.CustomerCoupon.ExpiryDate),
        nameof(CouponViewDto.ExpiryDate))]
    [MapProperty(
        $"{nameof(Repository.Models.Coupons.CustomerCoupon.Coupon)}.{nameof(Repository.Models.Coupons.Coupon.Id)}",
        nameof(CouponViewDto.Id))]
    [MapProperty(
        $"{nameof(Repository.Models.Coupons.CustomerCoupon.Coupon)}.{nameof(Repository.Models.Coupons.Coupon.Description)}",
        nameof(CouponViewDto.Description))]
    [MapProperty(
        $"{nameof(Repository.Models.Coupons.CustomerCoupon.Coupon)}.{nameof(Repository.Models.Coupons.Coupon.DiscountValue)}",
        nameof(CouponViewDto.DiscountValue))]
    [MapProperty(
        $"{nameof(Repository.Models.Coupons.CustomerCoupon.Coupon)}.{nameof(Repository.Models.Coupons.Coupon.DiscountType)}",
        nameof(CouponViewDto.DiscountType))]
    [MapProperty(
        $"{nameof(Repository.Models.Coupons.CustomerCoupon.Coupon)}.{nameof(Repository.Models.Coupons.Coupon.MaxDiscountAmount)}",
        nameof(CouponViewDto.MaxDiscountAmount))]
    public partial CouponViewDto ToCouponViewDto(Repository.Models.Coupons.CustomerCoupon customerCoupon);

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

    public partial List<CouponViewDto> ToCouponViewDtoList(
        List<Repository.Models.Coupons.CustomerCoupon> customerCoupons);
}