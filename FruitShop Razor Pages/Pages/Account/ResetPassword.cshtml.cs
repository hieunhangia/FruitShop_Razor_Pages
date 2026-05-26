using System.ComponentModel.DataAnnotations;
using FruitShop_Razor_Pages.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Service.Customer;
using Service.DTOs.Customer.Account;

namespace FruitShop_Razor_Pages.Pages.Account;

[LoggedInRedirectFilter]
public class ResetPasswordModel(AccountService accountService) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
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

    public async Task<IActionResult> OnGetAsync(string? userId, string? code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code) ||
            !await accountService.VerifyResetPasswordRequestAsync(new VerifyResetPasswordRequestDto
                { UserId = userId, Code = code }))
        {
            ViewData["Error"] = "Yêu cầu đặt lại mật khẩu không hợp lệ.";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string userId, string code)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await accountService.ResetPasswordAsync(new ResetPasswordDto
            {
                UserId = userId,
                Code = code,
                Password = Input.Password
            });
            TempData["SuccessMessage"] = "Mật khẩu đã được đặt lại thành công.";
            return RedirectToPage("Login");
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }
    }
}