using System.ComponentModel.DataAnnotations;
using FruitShop_Razor_Pages.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Models.Users;

namespace FruitShop_Razor_Pages.Pages.Account;

[LoggedInRedirectFilter]
public class LoginModel(SignInManager<User> signInManager) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl)
    {
        if (!ModelState.IsValid) return Page();
        var result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe,
            lockoutOnFailure: true);
        if (result.Succeeded)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage("/Index");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty,
                "Đăng nhập thất bại. Vui lòng kiểm tra email và mật khẩu của bạn.");
        }
        else if (result.IsNotAllowed)
        {
            ModelState.AddModelError(string.Empty,
                "Vui lòng xác nhận email của bạn để đăng nhập.\nNếu email xác nhận đã hết hạn, vui lòng chọn 'Gửi lại email xác nhận' để nhận email mới.");
        }
        else
        {
            ModelState.AddModelError(string.Empty,
                "Đăng nhập thất bại. Vui lòng kiểm tra email và mật khẩu của bạn.");
        }

        return Page();
    }
}