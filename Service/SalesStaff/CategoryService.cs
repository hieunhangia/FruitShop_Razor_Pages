using Microsoft.EntityFrameworkCore;
using Repository;
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
            query = query.Where(c => c.Name.Contains(request.Filter.SearchName));
        if (request.Filter.IsActive.HasValue)
            query = query.Where(c => c.IsActive == request.Filter.IsActive.Value);
        request.SortColumn ??= "Name";
        request.SortDirection ??= Repository.Constants.SortDirection.Ascending;
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
        return new PagedAndSortedDto<CategorySummaryDto>(items, totalCount, request.PageIndex, request.PageSize, request.SortColumn, request.SortDirection.Value);
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

    public async Task<List<Category>> GetAllCategoriesAsync()
    => await context.Categories.Where(c => c.IsActive).AsNoTracking().ToListAsync();

    public async Task CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            IsActive = true
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();
    }
}