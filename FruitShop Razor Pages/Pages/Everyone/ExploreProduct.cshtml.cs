using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.Constants;
using Service.DTOs;
using Service.DTOs.Everyone.Category;
using Service.DTOs.Everyone.Product;
using Service.Everyone;

namespace FruitShop_Razor_Pages.Pages.Everyone;

public class ExploreProductModel(ProductService productService, CategoryService categoryService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<ProductFilter> PagedAndSortedRequest { get; set; } = new();

    public PagedAndSortedDto<ProductSummaryDto>? PagedAndSortedResult { get; set; }

    public List<CategoryDto> Categories { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(bool? isSearch, string? searchTerm, int? categoryId, bool? bestSelling,
        bool? bestRating)
    {
        Categories = await categoryService.GetAllActiveCategoriesAsync();

        if (!ModelState.IsValid)
        {
            PagedAndSortedResult = new PagedAndSortedDto<ProductSummaryDto>([], 0, 0, 0,
                "", SortDirection.Ascending);
            return Page();
        }

        if (searchTerm != null)
        {
            PagedAndSortedRequest.Filter.SearchTerm = searchTerm;
        }

        if (categoryId != null)
        {
            PagedAndSortedRequest.Filter.CategoryId = categoryId;
        }

        if (bestSelling == true)
        {
            PagedAndSortedRequest.SortColumn = "BestSelling";
            PagedAndSortedRequest.SortDirection = SortDirection.Descending;
        }

        if (bestRating == true)
        {
            PagedAndSortedRequest.SortColumn = "BestRating";
            PagedAndSortedRequest.SortDirection = SortDirection.Descending;
        }

        if (isSearch == true)
        {
            PagedAndSortedRequest.PageIndex = 1;
        }

        PagedAndSortedRequest.PageSize = BusinessRuleConstants.ExploreProductPageValue.PageSize;

        PagedAndSortedResult = await productService.SearchProductsAsync(PagedAndSortedRequest);
        return Page();
    }
}