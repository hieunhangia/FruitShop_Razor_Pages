using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Repository.Models.Users;

namespace FruitShop_Razor_Pages.Pages.Account;

public class ConfirmEmailModel(UserManager<User> userManager) : PageModel
{
    public bool IsSuccess { get; set; }

    public async Task OnGetAsync(string? userId, string? code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            IsSuccess = false;
            return;
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            IsSuccess = false;
            return;
        }
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await userManager.ConfirmEmailAsync(user, code);
        IsSuccess = result.Succeeded;
    }
}