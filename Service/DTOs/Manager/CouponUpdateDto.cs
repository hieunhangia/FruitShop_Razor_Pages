using System.ComponentModel.DataAnnotations;
using Repository;
using Repository.Constants;

namespace Service.DTOs.Manager;

public class CouponUpdateDto
{
    public required int Id { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập mô tả.")]
    [StringLength(BusinessRuleConstants.Coupon.NameMaxLength,
        MinimumLength = BusinessRuleConstants.Coupon.NameMinLength,
        ErrorMessage = "Độ dài mô tả không hợp lệ.")]
    public required string Description { get; set; }
    public DiscountType DiscountType { get; set; }
    [Range(0, long.MaxValue, ErrorMessage = "Giá trị không hợp lệ.")]
    public long DiscountValue { get; set; }
    [Range(0, long.MaxValue, ErrorMessage = "Giá trị không hợp lệ.")]
    public long? MaxDiscountAmount { get; set; }
    [Range(0, long.MaxValue, ErrorMessage = "Giá trị không hợp lệ.")]
    public long? MinOrderAmount { get; set; }
    [Range(0, long.MaxValue, ErrorMessage = "Giá trị không hợp lệ.")]
    public long LoyaltyPointsCost { get; set; }
    
    public bool IsActive { get; set; }
    
}