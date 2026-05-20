using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.DTOs.SalesStaff.Product;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Product;

[Authorize(Roles = Role.SalesStaff)]
public class PriorityManagementModel(ProductService productService) : PageModel
{
    public List<ProductOrderingDto> Products { get; set; } = [];

    public async Task OnGetAsync()
    {
        Products = await productService.GetAllProductsForOrderingAsync();
    }

    public async Task<IActionResult> OnPostUpdatePriorityAsync([FromBody] List<int> productIds)
    {
        try
        {
            await productService.UpdatePrioritiesAsync(productIds);
            TempData["SuccessMessage"] = "Cập nhật thứ tự ưu tiên thành công!";
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Không thể cập nhật thứ tự: {ex.Message}";
            return new JsonResult(new { success = false });
        }
    }
}