using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;

namespace FruitShop_Razor_Pages.Pages.Customer.Order;

[Authorize(Roles = Role.Customer)]
public class OrderSuccessModel : PageModel;