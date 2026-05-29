using System.ComponentModel.DataAnnotations;
using FruitShop_Razor_Pages.Filters;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Models.Users;
using Service.Customer;
using Service.DTOs.Customer.Account;

namespace FruitShop_Razor_Pages.Pages.Account;

[LoggedInRedirectFilter]
public class LoginModel(AccountService accountService, SignInManager<User> signInManager) : PageModel
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
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl)
    {
        if (!ModelState.IsValid) return Page();
        try
        {
            await accountService.LoginAsync(new LoginDto
            {
                Email = Input.Email,
                Password = Input.Password
            });

            if (Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage("/Everyone/Index");
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }
    }

    public IActionResult OnPostGoogleLogin()
    {
        var properties = signInManager.ConfigureExternalAuthenticationProperties(
            GoogleOpenIdConnectDefaults.DisplayName, Url.Page("Login", pageHandler: "GoogleLoginCallback"));
        return Challenge(properties, GoogleOpenIdConnectDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> OnGetGoogleLoginCallbackAsync(string? remoteError)
    {
        if (remoteError != null)
        {
            TempData["ErrorMessage"] = $"Lỗi khi đăng nhập với Google: {remoteError}";
            return RedirectToPage();
        }

        try
        {
            await accountService.GoogleLoginAsync();
            return RedirectToPage("/Everyone/Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return RedirectToPage();
        }
    }
}