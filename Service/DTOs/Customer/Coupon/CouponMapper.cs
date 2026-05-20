using System.Linq;
using Riok.Mapperly.Abstractions;
using Coupons = Repository.Models.Coupons;

namespace Service.DTOs.Customer.Coupon;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, AutoUserMappings = false)]
public partial class CouponMapper
{
    [MapProperty(
        nameof(Coupons.CustomerCoupon.ExpiryDate),
        nameof(CouponViewDto.ExpiryDate))]
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
    public partial CouponViewDto ToCouponViewDto(Coupons.CustomerCoupon customerCoupon);


    [MapProperty(nameof(Coupons.Coupon.Description), nameof(CouponShopDto.Description))]
    [MapProperty(nameof(Coupons.Coupon.DiscountValue), nameof(CouponShopDto.DiscountValue))]
    [MapProperty(nameof(Coupons.Coupon.DiscountType), nameof(CouponShopDto.DiscountType))]
    [MapProperty(nameof(Coupons.Coupon.MaxDiscountAmount), nameof(CouponShopDto.MaxDiscountAmount))]
    [MapProperty(nameof(Coupons.Coupon.LoyaltyPointsCost), nameof(CouponShopDto.LoyaltyPointsCost))]
    public partial CouponShopDto ToCouponShopDto(Coupons.Coupon customerCoupon, bool canBuy);


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
    public partial CouponInCheckoutPageDto
        ToAvailableCouponForOrderDto(Coupons.CustomerCoupon customerCoupon);

    public partial List<CouponShopDto> ToCouponShopDtoList(List<Coupons.Coupon> customerCoupons);

    public partial List<CouponInCheckoutPageDto> ToAvailableCouponForOrderDtoList(
        List<Coupons.CustomerCoupon> customerCoupons);

    public partial List<CouponViewDto> ToCouponViewDtoList(
        List<Coupons.CustomerCoupon> customerCoupons);
}