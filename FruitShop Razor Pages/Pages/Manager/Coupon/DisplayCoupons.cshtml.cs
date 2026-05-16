using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Models.Coupons;

namespace FruitShop_Razor_Pages.Pages.Manager.Coupon;

[Authorize(Roles = Role.Manager)]
public class IndexModel(AppDbContext context) : PageModel
{
    public List<Repository.Models.Coupons.Coupon> Coupons { get; set; } = [];

    public async Task OnGetAsync()
    {
        Coupons = await context.Coupons.AsNoTracking()
            .OrderByDescending(c => c.IsActive)
            .ThenBy(c => c.Id)
            .ToListAsync();
    }
}

