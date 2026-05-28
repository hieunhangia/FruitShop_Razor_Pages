using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Service.DTOs;
using Service.DTOs.Everyone.Product;
using Service.Everyone;

namespace FruitShop_Razor_Pages.Pages.Everyone.Product;

public class ProductReviewModel(ProductService productService) : PageModel
{
    public ProductInReviewPageDto? Product { get; set; }

    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<ProductReviewFilter> PagedAndSortedRequest { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int productId, bool? isSearch)
    {
        if (!ModelState.IsValid)
        {
            Product = await productService.GetProductInReviewPageAsync(productId,
                new PagedAndSortedRequest<ProductReviewFilter>()
                {
                    PageSize = BusinessRuleConstants.ProductReviewPageValue.PageSize
                });
            return Page();
        }

        if (isSearch == true)
        {
            PagedAndSortedRequest.PageIndex = 1;
        }

        PagedAndSortedRequest.PageSize = BusinessRuleConstants.ProductReviewPageValue.PageSize;

        Product = await productService.GetProductInReviewPageAsync(productId, PagedAndSortedRequest);
        return Page();
    }
}