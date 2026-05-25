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
public class ProductEditModel(ProductService productService, CategoryService categoryService) : PageModel
{
    [BindProperty]
    public UpdateProductDto ProductUpdate { get; set; } = null!;

    [BindProperty]
    public IFormFile? ImageFile { get; set; }

    public int ProductId { get; set; }
    public string CurrentImageUrl { get; set; } = string.Empty;
    public List<ProductUnitDto> ProductUnits { get; set; } = [];
    public List<CategoryDto> Categories { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var productDetail = await productService.GetProductDetailAsync(id);
            ProductId = id;
            CurrentImageUrl = productDetail.ImageFilePath;

            ProductUpdate = new UpdateProductDto
            {
                Name = productDetail.Name,
                Description = productDetail.Description,
                Price = productDetail.Price,
                Quantity = productDetail.Quantity,
                ProductUnitId = productDetail.ProductUnitId, // BẮT BUỘC PHẢI CÓ DÒNG NÀY
                CategoryIds = productDetail.CategoryIds
            };

            ProductUnits = await productService.GetProductUnitsAsync();
            Categories = await categoryService.GetAllCategoriesAsync();
            return Page();
        }
        catch
        {
            return RedirectToPage("./ProductManagement");
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            await productService.UpdateProductFullAsync(id, ProductUpdate, ImageFile);
            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
            return RedirectToPage("./ProductDetail", new { id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ProductUnits = await productService.GetProductUnitsAsync();
            Categories = await categoryService.GetAllCategoriesAsync();
            ProductId = id;
            var productDetail = await productService.GetProductDetailAsync(id);
            CurrentImageUrl = productDetail.ImageFilePath;
            return Page();
        }
    }
}