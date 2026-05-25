using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Category;

[Authorize(Roles = Role.SalesStaff)]
public class CategoryPriorityManagementModel(CategoryService categoryService) : PageModel
{
    public List<CategoryOrderingDto> Categories { get; set; } = [];

    public async Task OnGetAsync()
    {
        Categories = await categoryService.GetAllCategoriesForOrderingAsync();
    }

    public async Task<IActionResult> OnPostUpdatePriorityAsync([FromBody] List<int> categoryIds)
    {
        try
        {
            await categoryService.UpdateCategoryPrioritiesAsync(categoryIds);
            TempData["SuccessMessage"] = "Cập nhật thứ tự ưu tiên danh mục thành công!";
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
            return new JsonResult(new { success = false });
        }
    }
}