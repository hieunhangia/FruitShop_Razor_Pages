using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.DTOs.SalesStaff.Category;
using Service.DTOs.SalesStaff.Product;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Product;

[Authorize(Roles = Role.SalesStaff)]
public class ProductCreateModel(ProductService productService, CategoryService categoryService) : PageModel
{
    [BindProperty]
    public CreateProductDto Product { get; set; } = null!;

    [BindProperty]
    public IFormFile ImageFile { get; set; } = null!;

    public List<ProductUnitDto> ProductUnits { get; set; } = [];
    public List<CategoryDto> Categories { get; set; } = [];

    public async Task OnGetAsync()
    {
        ProductUnits = await productService.GetProductUnitsAsync();
        Categories = await categoryService.GetAllCategoriesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ImageFile == null)
        {
            ModelState.AddModelError("ImageFile", "Vui lòng chọn hình ảnh cho sản phẩm.");
            await OnGetAsync();
            return Page();
        }

        try
        {
            await productService.CreateProductAsync(Product, ImageFile);
            TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
            return RedirectToPage("./ProductManagement");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await OnGetAsync();
            return Page();
        }
    }
}