using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs.Everyone.Category;

namespace Service.Everyone;

public class CategoryService(AppDbContext context, CategoryMapper mapper)
{
    public async Task<List<CategoryDto>> GetAllActiveCategoriesAsync() =>
        mapper.ToCategoryDtoList(await context.Categories.AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync());

    public async Task<List<CategoryDto>> GetTopActiveCategoriesAsync(int top)
        => mapper.ToCategoryDtoList(await context.Categories.AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Take(top)
            .ToListAsync());
}