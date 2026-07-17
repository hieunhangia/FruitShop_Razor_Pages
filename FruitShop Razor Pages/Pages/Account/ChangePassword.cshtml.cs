using System.ComponentModel.DataAnnotations;
using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.Models.Users;
using Service.Customer;
using Service.DTOs.Customer.Account;

namespace FruitShop_Razor_Pages.Pages.Account;

[Authorize]
public class ChangePasswordModel(AccountService accountService, UserManager<User> userManager) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại là bắt buộc.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
        [MinLength(BusinessRuleConstants.Identity.Password.RequiredLength,
            ErrorMessage = "Mật khẩu mới phải có ít nhất {1} ký tự.")]
        [MaxLength(BusinessRuleConstants.Identity.Password.MaxLength,
            ErrorMessage = "Mật khẩu mới không được vượt quá {1} ký tự.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu mới là bắt buộc.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp với mật khẩu mới.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!await userManager.HasPasswordAsync((await userManager.GetUserAsync(User))!))
        {
            return RedirectToPage("ManageAccount");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var userId = User.GetUserId();
            await accountService.ChangePasswordAsync(new ChangePasswordDto
            {
                UserId = userId.ToString(),
                CurrentPassword = Input.CurrentPassword,
                NewPassword = Input.NewPassword
            });
            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công.";
            return RedirectToPage();
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }
    }
}