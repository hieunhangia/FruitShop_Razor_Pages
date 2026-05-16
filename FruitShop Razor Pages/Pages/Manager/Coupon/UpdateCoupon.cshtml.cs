using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.Manager;

namespace FruitShop_Razor_Pages.Pages.Manager.Coupon;

[Authorize(Roles = Role.Manager)]
public class UpdateCouponModel(Service.Manager.CouponService couponService) : PageModel
{

    [BindProperty]
    public required CouponUpdateDto CouponUpdateDto { get; set; } 

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var coupon = await couponService.GetCouponByIdAsync(id);

        if (coupon == null)
        {
            return NotFound();
        }

        CouponUpdateDto = coupon;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid) {
            TempData["ErrorMessage"] = "Update chưa thành công. Vui lòng kiểm tra lại thông tin đã nhập.";
            return Page();
        }
        var result = await couponService.UpdateCouponAsync(id, CouponUpdateDto);
        if (result == null)
        {
            TempData["ErrorMessage"] = "Update chưa thành công. Vui lòng kiểm tra lại thông tin đã nhập.";
            return Page();
        }
        TempData["StatusMessage"] = "Update thành công";
        return RedirectToPage(new { id });
    }
    
}

