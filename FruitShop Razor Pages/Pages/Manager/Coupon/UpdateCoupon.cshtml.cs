using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.Constants;
using Service.DTOs.Manager;
using System.ComponentModel.DataAnnotations;

namespace FruitShop_Razor_Pages.Pages.Manager.Coupon;

[Authorize(Roles = Role.Manager)]
public class UpdateCouponModel(Service.Manager.CouponService couponService) : PageModel
{
    [BindProperty] public required InputModel Input { get; set; }

    public class InputModel
    {
        public required int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả.")]
        [MaxLength(BusinessRuleConstants.Model.Coupon.DescriptionMaxLength,
            ErrorMessage = "Độ dài mô tả không được vượt quá {1} ký tự.")]
        public required string Description { get; set; }

        public DiscountType DiscountType { get; set; }

        [Range(0, BusinessRuleConstants.Coupon.MaxDiscountValue,
            ErrorMessage = "Giá trị giảm giá phải nằm trong khoảng từ {1} đến {2:N0}")]
        public long DiscountValue { get; set; }

        [Range(0, BusinessRuleConstants.Coupon.MaxDiscountValue,
            ErrorMessage = "Giá trị giảm giá tối đa phải nằm trong khoảng từ {1} đến {2:N0}")]
        public long? MaxDiscountAmount { get; set; }

        [Range(0, BusinessRuleConstants.Coupon.MaxMinOrderAmount,
            ErrorMessage = "Giá trị đơn hàng tối thiểu phải nằm trong khoảng từ {1} đến {2:N0}")]
        public long? MinOrderAmount { get; set; }

        [Range(0, BusinessRuleConstants.Coupon.MaxLoyaltyPointsCost,
            ErrorMessage = "Giá trị điểm cần đổi phải nằm trong khoảng từ {1} đến {2:N0}")]
        public long LoyaltyPointsCost { get; set; }

        public bool IsActive { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var coupon = await couponService.GetCouponByIdAsync(id);

        if (coupon == null)
        {
            return NotFound();
        }

        Input = new InputModel
        {
            Id = coupon.Id,
            Description = coupon.Description,
            DiscountType = coupon.DiscountType,
            DiscountValue = coupon.DiscountValue,
            MaxDiscountAmount = coupon.MaxDiscountAmount,
            MinOrderAmount = coupon.MinOrderAmount,
            LoyaltyPointsCost = coupon.LoyaltyPointsCost,
            IsActive = coupon.IsActive
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var updateDto = new CouponUpdateDto
        {
            Id = id,
            Description = Input.Description,
            DiscountType = Input.DiscountType,
            DiscountValue = Input.DiscountValue,
            MaxDiscountAmount = Input.MaxDiscountAmount,
            MinOrderAmount = Input.MinOrderAmount,
            LoyaltyPointsCost = Input.LoyaltyPointsCost,
            IsActive = Input.IsActive
        };

        try
        {
            await couponService.UpdateCouponAsync(id, updateDto);
            TempData["SuccessMessage"] = "Cập nhật thành công.";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { id });
    }
}