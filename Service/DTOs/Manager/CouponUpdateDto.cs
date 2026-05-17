using System.ComponentModel.DataAnnotations;
using Repository;
using Repository.Constants;

namespace Service.DTOs.Manager;

public class CouponUpdateDto
{
    public required int Id { get; set; }
    public required string Description { get; set; }
    public DiscountType DiscountType { get; set; }
    public long DiscountValue { get; set; }
    public long? MaxDiscountAmount { get; set; }
    public long? MinOrderAmount { get; set; }
    public long LoyaltyPointsCost { get; set; }
    public bool IsActive { get; set; }
    
}