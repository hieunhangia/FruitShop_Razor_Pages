using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Data.Extensions;
using Repository.Models.Products;
using Service.DTOs;
using Service.DTOs.SalesStaff.Product;

namespace Service.SalesStaff;

public class ProductService(AppDbContext context, FileService fileService)
{
    public async Task<(int Total, int Active)> GetProductStatsAsync()
    {
        var total = await context.Products.CountAsync();
        var active = await context.Products.CountAsync(p => p.IsActive);
        return (total, active);
    }

    public async Task<PagedAndSortedDto<ProductSummaryDto>> GetProductsAsync(
        PagedAndSortedRequest<ProductFilter> request)
    {
        var query = context.Products
            .Include(p => p.ProductUnit)
            .Include(p => p.Categories)
            .AsNoTracking()
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Filter.SearchName))
            query = query.Where(p => p.Name.Contains(request.Filter.SearchName));
        if (request.Filter.CategoryId.HasValue)
            query = query.Where(p => p.Categories!.Any(c => c.Id == request.Filter.CategoryId.Value));
        if (request.Filter.IsActive.HasValue)
            query = query.Where(p => p.IsActive == request.Filter.IsActive.Value);
        request.SortColumn ??= "DisplayOrder";
        request.SortDirection ??= Repository.Constants.SortDirection.Ascending;
        var totalCount = await query.CountAsync();
        var items = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        var dtos = new List<ProductSummaryDto>();
        foreach (var p in items)
        {
            dtos.Add(new ProductSummaryDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                IsActive = p.IsActive,
                ImageFilePath = fileService.GetPublicFileUrl(p.ImageFilePath),
                ProductUnitName = p.ProductUnit!.Name
            });
        }

        return new PagedAndSortedDto<ProductSummaryDto>(dtos, totalCount, request.PageIndex, request.PageSize,
            request.SortColumn, request.SortDirection.Value);
    }

    public async Task ToggleProductStatusAsync(int id)
    {
        var product = await context.Products.FindAsync(id) ?? throw new Exception("Không tìm thấy sản phẩm.");
        product.IsActive = !product.IsActive;
        await context.SaveChangesAsync();
    }

    public async Task<ProductDetailDto> GetProductDetailAsync(int id)
    {
        var p = await context.Products
            .Include(x => x.ProductUnit)
            .Include(x => x.Categories)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Không tìm thấy sản phẩm");

        return new ProductDetailDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Quantity = p.Quantity,
            HeldQuantity = p.HeldQuantity,
            IsActive = p.IsActive,
            Description = p.Description,
            DisplayOrder = p.DisplayOrder,
            ImageFilePath = fileService.GetPublicFileUrl(p.ImageFilePath),
            ProductUnitName = p.ProductUnit!.Name,
            CategoryIds = p.Categories?.Select(c => c.Id).ToList() ?? [],
            CategoryNames = p.Categories?.Select(c => c.Name).ToList() ?? []
        };
    }

    public async Task UpdateProductBasicAsync(int id, string name, long price, int quantity, string description)
    {
        var product = await context.Products.FindAsync(id) ?? throw new Exception("Không tìm thấy sản phẩm");
        product.Name = name;
        product.Price = price;
        product.Quantity = quantity;
        product.Description = description;
        await context.SaveChangesAsync();
    }

    public async Task<List<ProductUnit>> GetProductUnitsAsync()
        => await context.ProductUnits.AsNoTracking().ToListAsync();

    public async Task CreateProductAsync(CreateProductDto dto, IFormFile imageFile)
    {
        var imagePath = await fileService.UploadProductImageAsync(imageFile);

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Quantity = dto.Quantity,
            DisplayOrder = dto.DisplayOrder,
            ProductUnitId = dto.ProductUnitId,
            ImageFilePath = imagePath,
            IsActive = true,
            HeldQuantity = 0,
            Categories = await context.Categories.Where(c => dto.CategoryIds.Contains(c.Id)).ToListAsync()
        };

        context.Products.Add(product);
        await context.SaveChangesAsync();
    }
}