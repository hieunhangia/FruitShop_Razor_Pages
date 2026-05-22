using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs;
using Service.DTOs.SalesStaff;
using Service.DTOs.SalesStaff.Category;
using Service.DTOs.SalesStaff.Product;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Product;

[Authorize(Roles = Role.SalesStaff)]
public class ProductManagementModel(ProductService productService, CategoryService categoryService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<ProductFilter> RequestData { get; set; } = new();

    public PagedAndSortedDto<ProductSummaryDto>? Products { get; set; }

    public List<CategoryDto> Categories { get; set; } = [];
    public List<ProductUnitDto> ProductUnits { get; set; } = [];

    public List<ProductOrderingDto> ProductsForOrdering { get; set; } = [];

    public async Task OnGetAsync()
    {
        Categories = await categoryService.GetAllCategoriesAsync();
        ProductUnits = await productService.GetProductUnitsAsync();
        Products = await productService.GetProductsAsync(RequestData);
        ProductsForOrdering = await productService.GetAllProductsForOrderingAsync();
    }

    public async Task<IActionResult> OnPostUpdatePriorityAsync([FromBody] List<int> productIds)
    {
        try
        {
            await productService.UpdatePrioritiesAsync(productIds);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }


}