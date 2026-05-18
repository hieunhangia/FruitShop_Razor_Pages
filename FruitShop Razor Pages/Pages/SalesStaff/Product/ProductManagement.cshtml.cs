using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs;
using Service.DTOs.SalesStaff;
using Service.DTOs.SalesStaff.Product;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Product;

[Authorize(Roles = Role.SalesStaff)]
public class ProductManagementModel(ProductService productService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<ProductFilter> RequestData { get; set; } = new();

    public PagedAndSortedDto<ProductSummaryDto>? Products { get; set; }

    public async Task OnGetAsync()
    {
        Products = await productService.GetProductsAsync(RequestData);
    }

}