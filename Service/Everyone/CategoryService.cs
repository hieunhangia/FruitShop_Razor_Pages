using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs.Everyone.Category;

namespace Service.Everyone;

public class CategoryService(AppDbContext context)
{
    public async Task<List<CategoryDto>> GetAllActiveCategoriesAsync() =>
        await context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ProjectToCategoryDto()
            .ToListAsync();

    public async Task<List<CategoryDto>> GetTopActiveCategoriesAsync(int top)
        => await context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Take(top)
            .ProjectToCategoryDto()
            .ToListAsync();
}