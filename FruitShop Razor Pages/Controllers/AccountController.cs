using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Models.Users;

namespace FruitShop_Razor_Pages.Controllers;

[Route("[controller]/[action]")]
[Authorize]
public class AccountController(SignInManager<User> signInManager) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Logout(string? returnUrl)
    {
        await signInManager.SignOutAsync();

        if (Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToPage("/Account/Login");
    }
}