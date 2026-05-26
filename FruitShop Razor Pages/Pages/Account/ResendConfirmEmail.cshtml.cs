using System.ComponentModel.DataAnnotations;
using FruitShop_Razor_Pages.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Customer;
using Service.DTOs.Customer.Account;

namespace FruitShop_Razor_Pages.Pages.Account;

[LoggedInRedirectFilter]
public class ResendConfirmEmailModel(AccountService accountService) : PageModel
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

        await accountService.ResendConfirmEmailAsync(new ResendConfirmEmailDto { Email = Input.Email },
            (userId, code) => Url.Page("/Account/ConfirmEmail", null, new { userId, code }, Request.Scheme)!);
        TempData["SuccessMessage"] =
            "Nếu tài khoản tồn tại và email chưa được xác nhận, một email xác nhận đã được gửi. Vui lòng kiểm tra email của bạn.";
        return RedirectToPage();
    }
}