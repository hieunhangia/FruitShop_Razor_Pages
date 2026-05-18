using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Dashboard;

[Authorize(Roles = Role.SalesStaff)]
public class IndexModel(ProductService productService, CategoryService categoryService) : PageModel
{
    public DashboardStatsDto Stats { get; set; } = new();

    public async Task OnGetAsync()
    {
        var productStats = await productService.GetProductStatsAsync();
        var categoryStats = await categoryService.GetCategoryStatsAsync();
        Stats = new DashboardStatsDto
        {
            TotalProducts = productStats.Total,
            ActiveProducts = productStats.Active,
            TotalCategories = categoryStats.Total,
            ActiveCategories = categoryStats.Active
        };
    }
}