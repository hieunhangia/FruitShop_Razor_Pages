using System.ComponentModel.DataAnnotations;
using Repository.Constants;

namespace Repository.Models.Coupons;

public class Coupon
{
    public int Id { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required long DiscountValue { get; set; }
    [Required] public required DiscountType DiscountType { get; set; }
    public long? MaxDiscountAmount { get; set; }
    public long? MinOrderAmount { get; set; }
    [Required] public required long LoyaltyPointsCost { get; set; }
    [Required] public bool IsActive { get; set; } = true;
}