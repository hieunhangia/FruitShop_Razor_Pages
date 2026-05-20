using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;

namespace FruitShop_Razor_Pages.Pages.Customer.Voucher;

[Authorize(Roles = Role.Customer)]
public class View : PageModel
{
    public async Task OnGetAsync()
    {
        
    }
}