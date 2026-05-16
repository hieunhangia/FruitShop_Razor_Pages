using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.DTOs.Everyone.Product;
using Service.Everyone;

namespace FruitShop_Razor_Pages.Pages.Everyone;

public class ProductDetailModel(ProductService productService) : PageModel
{
    public ProductDetailDto? Product { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Product = await productService.GetProductDetailByIdAsync(id);

        if (Product == null)
        {
            return NotFound();
        }

        return Page();
    }
}