using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FruitShop_Razor_Pages.Pages.Manager;

public class DashboardModel : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}