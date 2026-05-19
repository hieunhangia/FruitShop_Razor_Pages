using Repository.Constants;

namespace Service.DTOs.Manager;

public class CouponFilter
{
    public string? Keyword { get; set; }
    public DiscountType? DiscountType { get; set; }
    public bool? IsActive { get; set; }
}
