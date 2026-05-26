using System.ComponentModel.DataAnnotations;
using FruitShop_Razor_Pages.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Customer;
using Service.DTOs.Customer.Account;

namespace FruitShop_Razor_Pages.Pages.Account;

[LoggedInRedirectFilter]
public class LoginModel(AccountService accountService) : PageModel
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
        try
        {
            await accountService.LoginAsync(new LoginDto
            {
                Email = Input.Email,
                Password = Input.Password,
                RememberMe = Input.RememberMe
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
}