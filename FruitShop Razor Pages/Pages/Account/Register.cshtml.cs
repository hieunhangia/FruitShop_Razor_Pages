using System.ComponentModel.DataAnnotations;
using FruitShop_Razor_Pages.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Service.Customer;
using Service.DTOs.Customer.Account;

namespace FruitShop_Razor_Pages.Pages.Account;

[LoggedInRedirectFilter]
public class RegisterModel(AccountService accountService) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await accountService.RegisterAsync(new RegisterDto { Email = Input.Email, Password = Input.Password },
                (userId, code) => Url.Page("/Account/ConfirmEmail", null, new { userId, code }, Request.Scheme)!);
            TempData["SuccessMessage"] =
                "Đăng ký thành công! Vui lòng kiểm tra hòm thư email của bạn để xác nhận email.";
            return RedirectToPage();
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }
    }
}