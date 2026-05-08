using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FruitShop_Razor_Pages.Pages.Account;

[Authorize]
public class ManageAccountModel : PageModel;