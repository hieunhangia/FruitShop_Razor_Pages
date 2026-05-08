using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Models.Users;

namespace FruitShop_Razor_Pages.Pages.Account
{
    public class LoginModel(SignInManager<User> signInManager)
        : PageModel
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

            ModelState.AddModelError(string.Empty, result.IsLockedOut
                ? "Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau."
                : "Đăng nhập thất bại. Vui lòng kiểm tra email và mật khẩu của bạn.");

            return Page();
        }
    }
}