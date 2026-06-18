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
public class CreatePasswordModel(AccountService accountService, UserManager<User> userManager) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
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

    public async Task<IActionResult> OnGetAsync()
    {
        if (await userManager.HasPasswordAsync((await userManager.GetUserAsync(User))!))
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
            await accountService.CreatePasswordAsync(new CreatePasswordDto
                { UserId = User.GetUserId().ToString(), Password = Input.Password });
            TempData["SuccessMessage"] = "Tạo mật khẩu thành công!";
            return RedirectToPage("ManageAccount");
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }
    }
}