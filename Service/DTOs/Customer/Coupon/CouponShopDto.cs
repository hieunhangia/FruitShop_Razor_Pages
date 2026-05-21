using Repository.Constants;

namespace Service.DTOs.Customer.Coupon;

public class CouponShopDto
{
    public required int Id { get; set; }
    public required string Description { get; set; }
    public required long DiscountValue { get; set; }
    public required DiscountType DiscountType { get; set; }
    public required long? MaxDiscountAmount { get; set; }
    public required long LoyaltyPointsCost { get; set; }
    public required long? MinOrderAmount { get; set; }
}