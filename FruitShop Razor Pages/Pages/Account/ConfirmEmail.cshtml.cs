using FruitShop_Razor_Pages.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.Customer;
using Service.DTOs.Customer.Account;

namespace FruitShop_Razor_Pages.Pages.Account;

[LoggedInRedirectFilter]
public class ConfirmEmailModel(AccountService accountService) : PageModel
{
    public bool IsSuccess { get; set; }

    public async Task OnGetAsync(string userId, string code)
    {
        try
        {
            IsSuccess = await accountService.ConfirmEmailAsync(new ConfirmEmailDto { UserId = userId, Code = code });
        }
        catch
        {
            IsSuccess = false;
        }
    }
}