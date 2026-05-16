using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Models.Products;

namespace FruitShop_Razor_Pages.Pages.Products;

[Authorize(Roles = Role.SalesStaff)]
public class DetailsModel(AppDbContext db) : PageModel
{
    public Product? Product { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Product = await db.Products
            .Include(p => p.ProductUnit)
            .Include(p => p.Categories)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (Product == null)
        {
            return NotFound();
        }
        return Page();
    }
}