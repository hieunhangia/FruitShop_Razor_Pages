using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.DTOs.SalesStaff.Category;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Category;

[Authorize(Roles = Role.SalesStaff)]
public class CategoryDetailModel(CategoryService categoryService) : PageModel
{
    [BindProperty]
    public CategorySummaryDto Category { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Category = await categoryService.GetCategoryDetailAsync(id);
            return Page();
        }
        catch
        {
            return RedirectToPage("./CategoryManagement");
        }
    }

    public async Task<IActionResult> OnPostUpdateAsync(int id)
    {
        await categoryService.UpdateCategoryAsync(id, Category.Name);
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostToggleStatusAsync(int id)
    {
        await categoryService.ToggleCategoryStatusAsync(id);
        return RedirectToPage(new { id });
    }
}