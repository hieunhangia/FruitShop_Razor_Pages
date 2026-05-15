using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.DTOs.Guest.DetailProduct;
using Service.Guest;

namespace FruitShop_Razor_Pages.Pages.Guest;

public class ProductDetail(ProductService productService) : PageModel
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