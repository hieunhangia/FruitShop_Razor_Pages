using System.ComponentModel.DataAnnotations;
using System.Text;
using FruitShop_Razor_Pages.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Repository;
using Repository.Constants;
using Repository.Models.Users;
using Service;

namespace FruitShop_Razor_Pages.Pages.Account;

[LoggedInRedirectFilter]
public class RegisterModel(UserManager<User> userManager, EmailService emailService) : PageModel
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
        if (!ModelState.IsValid) return Page();

        var user = new User
        {
            UserName = Input.Email,
            Email = Input.Email
        };
        var result = await userManager.CreateAsync(user, Input.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.ToList();
            errors.RemoveAll(e => e.Code == "DuplicateUserName");
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        await userManager.AddToRoleAsync(user, Role.Customer);
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var confirmEmailUrl = Url.Page("/Account/ConfirmEmail", null, new { userId = user.Id, code }, Request.Scheme)!;
        _ = emailService.SendEmailAsync(Input.Email, "Xác nhận tài khoản",
            $"Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau: <a href='{confirmEmailUrl}'>Xác nhận email</a>.<br/>Liên kết này sẽ hết hạn sau {BusinessRuleConstants.Identity.TokenLifespan.EmailConfirmationMinutes} phút.");

        TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng kiểm tra email của bạn để xác nhận email.";
        return RedirectToPage();
    }
}