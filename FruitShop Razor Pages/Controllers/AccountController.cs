using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Models.Users;

namespace FruitShop_Razor_Pages.Controllers;

[Route("[controller]/[action]")]
[Authorize]
public class AccountController(SignInManager<User> signInManager) : Controller
{
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToPage("/Account/Login");
    }
}