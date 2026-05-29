using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Products;
using Service.DTOs;
using Service.DTOs.SalesStaff;
using Service.DTOs.SalesStaff.Category;

namespace Service.SalesStaff;

public class CategoryService(AppDbContext context)
{
    public async Task<(int Total, int Active)> GetCategoryStatsAsync()
    {
        var total = await context.Categories.CountAsync();
        var active = await context.Categories.CountAsync(c => c.IsActive);
        return (total, active);
    }

    public async Task<PagedAndSortedDto<CategorySummaryDto>> GetCategoriesAsync(
        PagedAndSortedRequest<CategoryFilter> request)
    {
        var query = context.Categories.Include(c => c.Products).AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Filter.SearchName))
            query = query.WhereContainsUnaccent(request.Filter.SearchName, c => c.Name);

        if (request.Filter.IsActive.HasValue)
            query = query.Where(c => c.IsActive == request.Filter.IsActive.Value);

        request.SortColumn ??= "DisplayOrder";
        request.SortDirection ??= SortDirection.Ascending;
        var totalCount = await query.CountAsync();
        var items = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CategorySummaryDto
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive,
                ProductCount = c.Products!.Count
            })
            .ToListAsync();
        return new PagedAndSortedDto<CategorySummaryDto>(items, totalCount, request.PageIndex, request.PageSize,
            request.SortColumn, request.SortDirection.Value);
    }

    public async Task ToggleCategoryStatusAsync(int id)
    {
        var category = await context.Categories.FindAsync(id) ?? throw new Exception("Không tìm thấy danh mục.");
        category.IsActive = !category.IsActive;
        await context.SaveChangesAsync();
    }

    public async Task<CategorySummaryDto> GetCategoryDetailAsync(int id)
    {
        var c = await context.Categories
            .Include(x => x.Products)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Không tìm thấy danh mục");

        return new CategorySummaryDto
        {
            Id = c.Id,
            Name = c.Name,
            IsActive = c.IsActive,
            ProductCount = c.Products!.Count
        };
    }

    public async Task UpdateCategoryAsync(int id, string name)
    {
        var category = await context.Categories.FindAsync(id) ?? throw new Exception("Không tìm thấy danh mục");
        category.Name = name;
        await context.SaveChangesAsync();
    }

    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        return await context.Categories
            .Where(c => c.IsActive)
            .AsNoTracking()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }

    public async Task<List<CategoryOrderingDto>> GetAllCategoriesForOrderingAsync()
    {
        return await context.Categories
            .OrderBy(c => c.DisplayOrder)
            .AsNoTracking()
            .Select(c => new CategoryOrderingDto
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive
            })
            .ToListAsync();
    }

    public async Task UpdateCategoryPrioritiesAsync(List<int> categoryIds)
    {
        var categories = await context.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();
        foreach (var cat in categories)
        {
            cat.DisplayOrder = -cat.Id;
        }
        await context.SaveChangesAsync();

        for (int i = 0; i < categoryIds.Count; i++)
        {
            var cat = categories.FirstOrDefault(c => c.Id == categoryIds[i]);
            if (cat != null)
            {
                cat.DisplayOrder = i + 1;
            }
        }
        await context.SaveChangesAsync();
    }

    public async Task CreateCategoryAsync(CreateCategoryDto dto)
    {
        if (await context.Categories.AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower()))
        {
            throw new Exception("Tên danh mục đã tồn tại.");
        }

        var maxDisplayOrder = await context.Categories.MaxAsync(c => (int?)c.DisplayOrder) ?? 0;
        var category = new Category
        {
            Name = dto.Name,
            IsActive = true,
            DisplayOrder = maxDisplayOrder + 1
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new Exception("Không tìm thấy danh mục.");
        return new CategoryDto { Id = category.Id, Name = category.Name, IsActive = category.IsActive };
    }

    public async Task UpdateCategoryAsync(int id, UpdateCategoryDto dto)
    {
        var category = await context.Categories.FindAsync(id)
            ?? throw new Exception("Không tìm thấy danh mục.");

        if (category.Name.ToLower() != dto.Name.ToLower() &&
            await context.Categories.AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower()))
        {
            throw new Exception("Tên danh mục đã tồn tại.");
        }

        category.Name = dto.Name;
        category.IsActive = dto.IsActive;
        await context.SaveChangesAsync();
    }

}