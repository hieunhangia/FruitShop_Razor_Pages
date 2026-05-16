using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
namespace FruitShop_Razor_Pages.Pages.Manager.Coupon;

[Authorize(Roles = Role.Manager)]
public class DetailModel(AppDbContext context) : PageModel
{
    public Repository.Models.Coupons.Coupon? Coupon { get; private set; }

    [BindProperty]
    public CouponInputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var coupon = await context.Coupons.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (coupon == null)
        {
            return NotFound();
        }

        Coupon = coupon;
        Input = CouponInputModel.FromEntity(coupon);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.Id == id);
        if (coupon == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            Coupon = coupon;
            return Page();
        }

        coupon.Description = Input.Description.Trim();
        coupon.DiscountType = Input.DiscountType;
        coupon.DiscountValue = Input.DiscountValue;
        coupon.MaxDiscountAmount = Input.MaxDiscountAmount;
        coupon.MinOrderAmount = Input.MinOrderAmount;
        coupon.LoyaltyPointsCost = Input.LoyaltyPointsCost;
        coupon.IsActive = Input.IsActive;

        await context.SaveChangesAsync();

        TempData["StatusMessage"] = "Đã cập nhật mã giảm giá.";
        return RedirectToPage(new { id });
    }

    public class CouponInputModel
    {
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Vui lòng nhập mô tả.")]
        [StringLength(BusinessRuleConstants.Coupon.NameMaxLength,
            MinimumLength = BusinessRuleConstants.Coupon.NameMinLength,
            ErrorMessage = "Độ dài mô tả không hợp lệ.")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Loại giảm")]
        [Required(ErrorMessage = "Vui lòng chọn loại giảm giá.")]
        public DiscountType DiscountType { get; set; }

        [Display(Name = "Giá trị")]
        [Required(ErrorMessage = "Vui lòng nhập giá trị giảm.")]
        [Range(0, long.MaxValue, ErrorMessage = "Giá trị không hợp lệ.")]
        public long DiscountValue { get; set; }

        [Display(Name = "Giảm tối đa")]
        [Range(0, long.MaxValue, ErrorMessage = "Giá trị không hợp lệ.")]
        public long? MaxDiscountAmount { get; set; }

        [Display(Name = "Đơn tối thiểu")]
        [Range(0, long.MaxValue, ErrorMessage = "Giá trị không hợp lệ.")]
        public long? MinOrderAmount { get; set; }

        [Display(Name = "Điểm đổi")]
        [Required(ErrorMessage = "Vui lòng nhập điểm đổi.")]
        [Range(0, long.MaxValue, ErrorMessage = "Giá trị không hợp lệ.")]
        public long LoyaltyPointsCost { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        public static CouponInputModel FromEntity(Repository.Models.Coupons.Coupon coupon)
        {
            return new CouponInputModel
            {
                Description = coupon.Description,
                DiscountType = coupon.DiscountType,
                DiscountValue = coupon.DiscountValue,
                MaxDiscountAmount = coupon.MaxDiscountAmount,
                MinOrderAmount = coupon.MinOrderAmount,
                LoyaltyPointsCost = coupon.LoyaltyPointsCost,
                IsActive = coupon.IsActive
            };
        }
    }
}

