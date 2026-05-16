using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Models.Products;

namespace FruitShop_Razor_Pages.Pages.SalesStaff;

[Authorize(Roles = Role.SalesStaff)]
public class DashboardModel(AppDbContext db) : PageModel
{
    public List<Product> Products { get; set; } = [];
    public List<Category> Categories { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string CurrentTab { get; set; } = "Product";

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task OnGetAsync()
    {
        if (CurrentTab == "Product")
        {
            var productQuery = db.Products
                .Include(p => p.ProductUnit)
                .Include(p => p.Categories)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                productQuery = productQuery.Where(p =>
                    p.Name.Contains(SearchTerm) ||
                    p.Categories.Any(c => c.Name.Contains(SearchTerm)));
            }

            Products = await productQuery.ToListAsync();
        }
        else
        {
            var categoryQuery = db.Categories.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                categoryQuery = categoryQuery.Where(c => c.Name.Contains(SearchTerm));
            }

            Categories = await categoryQuery.ToListAsync();
        }
    }
}