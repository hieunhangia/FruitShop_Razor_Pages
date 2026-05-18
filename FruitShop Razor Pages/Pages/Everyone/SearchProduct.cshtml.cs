using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.DTOs.Everyone.Product;
using Service.Everyone;

namespace FruitShop_Razor_Pages.Pages.Everyone;

public class SearchProductModel(ProductService productService) : PageModel
{
    [BindProperty(SupportsGet = true)] public string SearchTerm { get; set; } = string.Empty;

    public List<ProductSummaryDto> SearchResults { get; set; } = [];

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            SearchResults = await productService.SearchProductsAsync(SearchTerm);
        }
    }
}