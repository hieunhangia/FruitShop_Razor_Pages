using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff;
using Service.DTOs.SalesStaff.Category;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Category;

[Authorize(Roles = Role.SalesStaff)]
public class CategoryCreateModel(CategoryService categoryService) : PageModel
{
    [BindProperty]
    public CreateCategoryDto Category { get; set; } = null!;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await categoryService.CreateCategoryAsync(Category);
            TempData["SuccessMessage"] = "Thêm danh m?c thành công!";
            return RedirectToPage("./CategoryManagement");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return Page();
        }
    }
}