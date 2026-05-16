using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Models.Products;

namespace FruitShop_Razor_Pages.Pages.Products;

[Authorize(Roles = Role.SalesStaff)]
public class EditModel(AppDbContext db) : PageModel
{
    [BindProperty]
    public Product Product { get; set; } = default!;

    [BindProperty]
    public List<int> SelectedCategoryIds { get; set; } = [];

    public SelectList UnitOptions { get; set; } = default!;
    public List<Category> CategoryOptions { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var product = await db.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (product == null) return NotFound();

        Product = product;
        SelectedCategoryIds = product.Categories?.Select(c => c.Id).ToList() ?? new List<int>();

        await LoadSelectLists();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin nhập.";
            await LoadSelectLists();
            return Page();
        }

        var productToUpdate = await db.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == Product.Id);

        if (productToUpdate == null) return NotFound();

        productToUpdate.Name = Product.Name;
        productToUpdate.Description = Product.Description;
        productToUpdate.Price = Product.Price;
        productToUpdate.Quantity = Product.Quantity;
        productToUpdate.ImageUrl = Product.ImageUrl;
        productToUpdate.IsActive = Product.IsActive;
        productToUpdate.ProductUnitId = Product.ProductUnitId;

        productToUpdate.Categories?.Clear();
        if (SelectedCategoryIds.Any())
        {
            var newCategories = await db.Categories
                .Where(c => SelectedCategoryIds.Contains(c.Id))
                .ToListAsync();

            foreach (var cat in newCategories)
            {
                productToUpdate.Categories?.Add(cat);
            }
        }

        try
        {
            await db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật thông tin sản phẩm thành công.";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!db.Products.Any(e => e.Id == Product.Id)) return NotFound();
            else throw;
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            await LoadSelectLists();
            return Page();
        }

        return RedirectToPage("/SalesStaff/Dashboard", new { CurrentTab = "Product" });
    }

    private async Task LoadSelectLists()
    {
        var units = await db.ProductUnits.ToListAsync();
        UnitOptions = new SelectList(units, "Id", "Name");
        CategoryOptions = await db.Categories.Where(c => c.IsActive).ToListAsync();
    }
}