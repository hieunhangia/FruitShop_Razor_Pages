using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.DTOs;
using Service.DTOs.SalesStaff;
using Service.DTOs.SalesStaff.Category;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Category;

[Authorize(Roles = "SalesStaff")]
public class CategoryManagementModel(CategoryService categoryService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<CategoryFilter> RequestData { get; set; } = new();

    public PagedAndSortedDto<CategorySummaryDto>? Categories { get; set; }

    public async Task OnGetAsync()
    {
        Categories = await categoryService.GetCategoriesAsync(RequestData);
    }

    public async Task<IActionResult> OnPostToggleStatusAsync(int id)
    {
        try
        {
            await categoryService.ToggleCategoryStatusAsync(id);
            TempData["SuccessMessage"] = "Cập nhật trạng thái thành công!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToPage();
    }


}