using Repository.Constants;

namespace Service.DTOs.Manager;

public class CouponFilter
{
    public string? Keyword { get; set; }
    public DiscountType? DiscountType { get; set; }
    public bool? IsActive { get; set; }
    public long? StartLoyaltyPointsCost { get; set; }
    public long? EndLoyaltyPointsCost { get; set; }
}
