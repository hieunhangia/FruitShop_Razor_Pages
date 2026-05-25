using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Category;

[Authorize(Roles = Role.SalesStaff)]
public class CategoryEditModel(CategoryService categoryService) : PageModel
{
    [BindProperty]
    public UpdateCategoryDto CategoryUpdate { get; set; } = null!;

    public int CategoryId { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            CategoryId = id;

            CategoryUpdate = new UpdateCategoryDto
            {
                Name = category.Name,
                IsActive = category.IsActive
            };
            return Page();
        }
        catch
        {
            return RedirectToPage("./CategoryManagement");
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await categoryService.UpdateCategoryAsync(id, CategoryUpdate);
            TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
            return RedirectToPage("./CategoryManagement");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            CategoryId = id;
            return Page();
        }
    }
}