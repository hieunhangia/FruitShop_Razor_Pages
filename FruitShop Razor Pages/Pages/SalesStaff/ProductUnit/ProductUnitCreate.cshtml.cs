using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.ProductUnit;

[Authorize(Roles = Role.SalesStaff)]
public class ProductUnitCreateModel(ProductService productService) : PageModel
{
    [BindProperty]
    public CreateProductUnitDto Unit { get; set; } = null!;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await productService.CreateProductUnitAsync(Unit);
            TempData["SuccessMessage"] = "Thêm đơn vị tính thành công!";
            return RedirectToPage("./ProductUnitManagement");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return Page();
        }
    }
}