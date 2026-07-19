using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.DTOs.SalesStaff.Product;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.ProductUnit;

[Authorize(Roles = Role.SalesStaff)]
public class ProductUnitManagementModel(ProductService productService) : PageModel
{
    public List<ProductUnitDto> Units { get; set; } = [];

    public async Task OnGetAsync()
    {
        Units = await productService.GetProductUnitsAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            await productService.DeleteProductUnitAsync(id);
            TempData["SuccessMessage"] = "Xóa đơn vị tính thành công!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostToggleAsync(int id)
    {
        try
        {
            await productService.ToggleProductUnitActiveAsync(id);
            TempData["SuccessMessage"] = "Cập nhật trạng thái đơn vị tính thành công!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToPage();
    }
}