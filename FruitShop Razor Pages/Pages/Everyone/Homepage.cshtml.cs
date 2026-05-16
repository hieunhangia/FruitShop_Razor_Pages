using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.DTOs.Everyone.Product;
using Service.Everyone;

namespace FruitShop_Razor_Pages.Pages.Everyone;

public class HomepageModel(ProductService productService) : PageModel
{
    public List<ProductSummaryDto> NewestProducts { get; set; } = [];
    public List<ProductSummaryDto> ProductsByPrice { get; set; } = [];

    public async Task OnGetAsync()
    {
        NewestProducts = await productService.GetNewestProductsAsync();
        ProductsByPrice = await productService.GetProductsByPriceAsync();
    }
}