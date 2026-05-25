using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Service.DTOs.Everyone.Category;
using Service.DTOs.Everyone.Product;
using Service.Everyone;

namespace FruitShop_Razor_Pages.Pages.Everyone;

public class IndexModel(ProductService productService, CategoryService categoryService) : PageModel
{
    public List<CategoryDto> TopCategories { get; set; } = [];
    public List<ProductSummaryDto> TopProducts { get; set; } = [];
    public List<ProductSummaryDto> BestSellingProducts { get; set; } = [];
    public List<ProductSummaryDto> BestRatingProducts { get; set; } = [];
    public List<ProductReviewDto> TopProductReviews { get; set; } = [];

    public async Task OnGetAsync()
    {
        TopCategories = await categoryService.GetTopCategoriesAsync(
            BusinessRuleConstants.IndexPageValue.NumberOfTopCategory);
        TopProducts = await productService.GetTopProductsAsync(BusinessRuleConstants.IndexPageValue.NumberOfTopProduct);
        BestSellingProducts = await productService.GetBestSellingProductsAsync(
            BusinessRuleConstants.IndexPageValue.NumberOfBestSellingProduct);
        BestRatingProducts = await productService.GetBestRatingProductsAsync(
            BusinessRuleConstants.IndexPageValue.NumberOfBestRatingProduct);
        TopProductReviews = await productService.GetTopProductReviewsAsync(
            BusinessRuleConstants.IndexPageValue.NumberOfTopProductReview);
    }
}