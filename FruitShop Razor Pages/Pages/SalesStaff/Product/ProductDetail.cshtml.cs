using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.DTOs.SalesStaff.Product;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Product;

[Authorize(Roles = Role.SalesStaff)]
public class ProductDetail(ProductService productService) : PageModel
{
    [BindProperty]
    public ProductDetailDto Product { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Product = await productService.GetProductDetailAsync(id);
            //ViewData
            return Page();
        }
        catch
        {
            return RedirectToPage("./ProductManagement");
        }
    }

    public async Task<IActionResult> OnPostUpdateAsync(int id)
    {
        await productService.UpdateProductBasicAsync(id, Product.Name, Product.Price, Product.Quantity, Product.Description);
        TempData["SuccessMessage"] = "Đã lưu thành công!";
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostToggleStatusAsync(int id)
    {
        await productService.ToggleProductStatusAsync(id);
        return RedirectToPage(new { id });
    }
}