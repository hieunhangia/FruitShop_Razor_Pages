using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Repository.Models.Users;
using Service;

namespace FruitShop_Razor_Pages.Pages.Account;

public class ForgotPassword(UserManager<User> userManager, EmailService emailService) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await userManager.FindByEmailAsync(Input.Email);
        if (user is not { EmailConfirmed: true })
        {
            TempData["SuccessMessage"] =
                "Nếu tài khoản tồn tại và đã được xác nhận, một email với hướng dẫn đặt lại mật khẩu đã được gửi. Vui lòng kiểm tra email của bạn.";
            return Page();
        }

        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page("/Account/ResetPassword", null, new { userId = user.Id, code }, Request.Scheme)!;

        _ = emailService.SendEmailAsync(Input.Email, "Đặt lại mật khẩu",
            $"Vui lòng đặt lại mật khẩu của bạn bằng cách <a href='{callbackUrl}'>nhấn vào đây</a>.");

        TempData["SuccessMessage"] =
            "Nếu tài khoản tồn tại và đã được xác nhận, một email với hướng dẫn đặt lại mật khẩu đã được gửi. Vui lòng kiểm tra email của bạn.";
        return RedirectToPage();
    }
}