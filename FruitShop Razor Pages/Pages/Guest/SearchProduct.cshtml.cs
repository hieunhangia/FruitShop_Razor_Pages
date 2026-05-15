using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.DTOs.Guest.Homepage;
using Service.Guest;

namespace FruitShop_Razor_Pages.Pages.Guest;

public class SearchProduct(ProductService productService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;
    
    public List<ProductDto> SearchResults { get; set; } = [];

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            SearchResults = await productService.SearchProductsAsync(SearchTerm);
        }
    }
}