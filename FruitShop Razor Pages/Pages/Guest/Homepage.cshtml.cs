using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Models.Products;

namespace FruitShop_Razor_Pages.Pages.Guest;

public class Homepage(AppDbContext db) : PageModel
{
    public List<Product> NewestProducts { get; set; } = [];
    public List<Product> ProductsByPrice { get; set; } = [];

    public async Task OnGetAsync()
    {
        var activeProducts = db.Products
            .Include(p => p.ProductUnit)
            .Where(p => p.IsActive);

        NewestProducts = await activeProducts
            .OrderByDescending(p => p.Id)
            .Take(8)
            .ToListAsync();

        ProductsByPrice = await activeProducts
            .OrderBy(p => p.Price)
            .Take(8)
            .ToListAsync();
    }
}