using System.ComponentModel.DataAnnotations;
using Repository.Models.Users;

namespace Repository.Models.Coupons;

public class CustomerCoupon
{
    [Required] public bool IsUsed { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public Guid CustomerId { get; set; }
    public User? Customer { get; set; }

    public Guid CouponId { get; set; }
    public Coupon? Coupon { get; set; }
}