using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Repository.Constants;
using Service.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.DTOs.Everyone.Product;
using Service.Everyone;

namespace FruitShop_Razor_Pages.Pages.Everyone;

public class ProductDetailModel(ProductService productService, CartService cartService) : PageModel
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
    
    public async Task<IActionResult> OnPostAddToCartAsync(int id)
    {
        var customerId = User.GetUserId();
        try
        {
            await cartService.AddQuantityForProductCart(customerId, id);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage("/Customer/Cart");
    }
}