using System.ComponentModel.DataAnnotations;
using Repository.Models.Users;

namespace Repository.Models.Coupons;

public class CustomerCoupon
{
    public int Id { get; set; }
    [Required] public bool IsUsed { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public int CustomerId { get; set; }
    public CustomerData? Customer { get; set; }

    public int CouponId { get; set; }
    public Coupon? Coupon { get; set; }
}