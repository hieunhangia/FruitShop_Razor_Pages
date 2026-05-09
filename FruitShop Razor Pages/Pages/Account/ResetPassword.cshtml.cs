using System.ComponentModel.DataAnnotations;
using System.Text;
using FruitShop_Razor_Pages.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Repository;
using Repository.Models.Users;

namespace FruitShop_Razor_Pages.Pages.Account;

[LoggedInRedirectFilter]
public class ResetPasswordModel(UserManager<User> userManager) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(BusinessRuleConstants.Identity.Password.RequiredLength,
            ErrorMessage = "Mật khẩu phải có ít nhất {1} ký tự.")]
        [MaxLength(BusinessRuleConstants.Identity.Password.MaxLength,
            ErrorMessage = "Mật khẩu không được vượt quá {1} ký tự.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public IActionResult OnGet(string? userId, string? code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            return BadRequest("Yêu cầu đặt lại mật khẩu không hợp lệ.");
        }

        Input.UserId = userId;
        Input.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await userManager.FindByIdAsync(Input.UserId);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Đặt lại mật khẩu thất bại. Vui lòng thử lại.");
            return Page();
        }

        var result = await userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        TempData["SuccessMessage"] = "Mật khẩu đã được đặt lại thành công.";
        return RedirectToPage("Login");
    }
}