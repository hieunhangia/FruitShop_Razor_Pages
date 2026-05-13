using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.DTOs.Guest;
using Service.DTOs.Guest.Homepage;
using Service.Guest;

namespace FruitShop_Razor_Pages.Pages.Guest;

public class Homepage(HomepageService homepageService) : PageModel
{
    public List<ProductDto> NewestProducts { get; set; } = [];
    public List<ProductDto> ProductsByPrice { get; set; } = [];

    public async Task OnGetAsync()
    {
        NewestProducts = await homepageService.GetNewestProductsAsync();
        ProductsByPrice = await homepageService.GetProductsByPriceAsync();
    }
}