using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.ProductUnit;

[Authorize(Roles = Role.SalesStaff)]
public class ProductUnitEditModel(ProductService productService) : PageModel
{
    [BindProperty]
    public UpdateProductUnitDto UnitUpdate { get; set; } = null!;
    public int UnitId { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var unit = await productService.GetProductUnitByIdAsync(id);
            UnitId = id;
            UnitUpdate = new UpdateProductUnitDto { Name = unit.Name };
            return Page();
        }
        catch
        {
            return RedirectToPage("./ProductUnitManagement");
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            await productService.UpdateProductUnitAsync(id, UnitUpdate);
            TempData["SuccessMessage"] = "Cập nhật đơn vị tính thành công!";
            return RedirectToPage("./ProductUnitManagement");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            UnitId = id;
            return Page();
        }
    }
}