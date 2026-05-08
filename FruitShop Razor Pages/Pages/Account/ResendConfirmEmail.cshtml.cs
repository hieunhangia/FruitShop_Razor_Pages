using System.ComponentModel.DataAnnotations;
using System.Text;
using FruitShop_Razor_Pages.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Repository.Models.Users;
using Service;

namespace FruitShop_Razor_Pages.Pages.Account;

[LoggedInRedirectFilter]
public class ResendConfirmEmailModel(UserManager<User> userManager, EmailService emailService) : PageModel
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
        if (user == null || user.EmailConfirmed)
        {
            TempData["SuccessMessage"] =
                "Nếu tài khoản tồn tại và email chưa được xác nhận, một email xác nhận đã được gửi. Vui lòng kiểm tra email của bạn.";
            return RedirectToPage();
        }

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var confirmEmailUrl = Url.Page("/Account/ConfirmEmail", null, new { userId = user.Id, code }, Request.Scheme)!;
        _ = emailService.SendEmailAsync(Input.Email, "Xác nhận tài khoản",
            $"Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau: <a href='{confirmEmailUrl}'>Xác nhận email</a>");

        TempData["SuccessMessage"] =
            "Nếu tài khoản tồn tại và email chưa được xác nhận, một email xác nhận đã được gửi. Vui lòng kiểm tra email của bạn.";
        return RedirectToPage();
    }
}