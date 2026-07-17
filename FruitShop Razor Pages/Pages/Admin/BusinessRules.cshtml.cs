using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service;
using BusinessRuleModel = Repository.Models.BusinessRule.BusinessRule;

namespace FruitShop_Razor_Pages.Pages.Admin;

[Authorize(Roles = Role.Admin)]
public class BusinessRulesModel(BusinessRuleService businessRuleService) : PageModel
{
    public List<BusinessRuleModel> Rules { get; set; } = [];

    [BindProperty]
    public string UpdateType { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Giá trị là bắt buộc.")]
    public string UpdateValue { get; set; } = string.Empty;

    // Thứ tự các nhóm hiển thị trên trang.
    public static readonly string[] GroupOrder =
    [
        "Bảo mật & Thanh toán",
        "Điểm thưởng (Loyalty)"
    ];

    // Metadata trình bày (nhóm + đơn vị) gắn 1-1 với enum, không lưu DB.
    public static string GetGroup(BusinessRuleConstantType type) => type switch
    {
        BusinessRuleConstantType.PrivateFileUrlExpirationSeconds => "Bảo mật & Thanh toán",
        BusinessRuleConstantType.QrCodePaymentOrderExpiredMinutes => "Bảo mật & Thanh toán",
        _ => "Điểm thưởng (Loyalty)"
    };

    public static string GetUnit(BusinessRuleConstantType type) => type switch
    {
        BusinessRuleConstantType.PrivateFileUrlExpirationSeconds => "giây",
        BusinessRuleConstantType.QrCodePaymentOrderExpiredMinutes => "phút",
        BusinessRuleConstantType.LoyaltyPointEarnedWhenRegister => "điểm",
        BusinessRuleConstantType.LoyaltyPointEarnedPerComment => "điểm",
        BusinessRuleConstantType.VNDPerLoyaltyPoint => "VND",
        _ => string.Empty
    };

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Rules = await businessRuleService.GetAllAsync();
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateAsync()
    {
        if (!ModelState.IsValid)
        {
            try { Rules = await businessRuleService.GetAllAsync(); } catch { /* ignore */ }
            return Page();
        }

        try
        {
            if (!Enum.TryParse<BusinessRuleConstantType>(UpdateType, out var type))
                throw new ArgumentException($"Loại quy tắc không hợp lệ: {UpdateType}");

            await businessRuleService.SetValueAsync(type, UpdateValue);
            TempData["SuccessMessage"] = "Cập nhật quy tắc nghiệp vụ thành công.";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage();
    }
}
