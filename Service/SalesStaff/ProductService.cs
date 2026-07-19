using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Data.Extensions;
using Repository.Models.Products;
using Service.DTOs;
using Service.DTOs.SalesStaff;
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


    public async Task<PagedAndSortedDto<ProductSummaryDto>> GetProductsAsync(PagedAndSortedRequest<ProductFilter> request)
    {
        var query = context.Products
            .Include(p => p.ProductUnit)
            .Include(p => p.Categories)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Filter.SearchName))
        {
            query = query.WhereContainsUnaccent(request.Filter.SearchName, p => p.Name);
        }

        if (request.Filter.CategoryIds.Count > 0)
        {
            query = query.Where(p => p.Categories!.Any(c => request.Filter.CategoryIds.Contains(c.Id)));
        }
        if (request.Filter.ProductUnitId.HasValue)
            query = query.Where(p => p.ProductUnitId == request.Filter.ProductUnitId.Value);

        if (request.Filter.IsActive.HasValue)
            query = query.Where(p => p.IsActive == request.Filter.IsActive.Value);

        if (request.Filter.PriceFrom.HasValue)
            query = query.Where(p => p.Price >= request.Filter.PriceFrom.Value);

        if (request.Filter.PriceTo.HasValue)
            query = query.Where(p => p.Price <= request.Filter.PriceTo.Value);

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
        return new PagedAndSortedDto<ProductSummaryDto>(dtos, totalCount, request.PageIndex, request.PageSize, request.SortColumn, request.SortDirection.Value);
    }

    public async Task UpdateProductFullAsync(int id, UpdateProductDto dto, IFormFile? newImageFile)
    {
        var product = await context.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("Không tìm thấy sản phẩm.");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Quantity = dto.Quantity;
        product.ProductUnitId = dto.ProductUnitId;

        if (newImageFile != null && newImageFile.Length > 0)
        {
            if (!string.IsNullOrWhiteSpace(product.ImageFilePath))
            {
                try
                {
                    await fileService.DeleteFileAsync(product.ImageFilePath);
                }
                catch
                {
                }
            }

            var newImagePath = await fileService.UploadProductImageAsync(newImageFile);
            product.ImageFilePath = newImagePath;
        }

        product.Categories!.Clear();
        if (dto.CategoryIds.Count > 0)
        {
            var newCategories = await context.Categories
                .Where(c => dto.CategoryIds.Contains(c.Id))
                .ToListAsync();
            foreach (var cat in newCategories)
            {
                product.Categories.Add(cat);
            }
        }

        await context.SaveChangesAsync();
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
            ProductUnitId= p.ProductUnitId,
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

    public async Task<List<ProductUnitDto>> GetProductUnitsAsync()
    {
        return await context.ProductUnits.AsNoTracking()
            .Select(u => new ProductUnitDto
            {
                Id = u.Id,
                Name = u.Name,
                IsActive = u.IsActive
            })
            .ToListAsync();
    }

    public async Task CreateProductAsync(CreateProductDto dto, IFormFile imageFile)
    {
        if (await context.Products.AnyAsync(p => p.Name.ToLower() == dto.Name.ToLower()))
        {
            throw new Exception("Tên sản phẩm đã tồn tại.");
        }

        var maxDisplayOrder = await context.Products.MaxAsync(p => (int?)p.DisplayOrder) ?? 0;

        var imagePath = await fileService.UploadProductImageAsync(imageFile);

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Quantity = dto.Quantity,
            DisplayOrder = maxDisplayOrder + 1,
            ProductUnitId = dto.ProductUnitId,
            ImageFilePath = imagePath,
            IsActive = true,
            HeldQuantity = 0,
            Categories = await context.Categories.Where(c => dto.CategoryIds.Contains(c.Id)).ToListAsync()
        };

        context.Products.Add(product);
        await context.SaveChangesAsync();
    }

    public async Task<List<ProductOrderingDto>> GetAllProductsForOrderingAsync()
    {
        var products = await context.Products
            .OrderBy(p => p.DisplayOrder)
            .AsNoTracking()
            .ToListAsync();

        var dtos = new List<ProductOrderingDto>();
        foreach (var p in products)
        {
            dtos.Add(new ProductOrderingDto
            {
                Id = p.Id,
                Name = p.Name,
                ImageFilePath = fileService.GetPublicFileUrl(p.ImageFilePath),
                IsActive = p.IsActive
            });
        }
        return dtos;
    }

    public async Task UpdatePrioritiesAsync(List<int> productIds)
    {
        var products = await context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

        foreach (var product in products)
        {
            product.DisplayOrder = -product.Id;
        }
        await context.SaveChangesAsync();

        for (int i = 0; i < productIds.Count; i++)
        {
            var product = products.FirstOrDefault(p => p.Id == productIds[i]);
            if (product != null)
            {
                product.DisplayOrder = i + 1;
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<ProductUnitDto> GetProductUnitByIdAsync(int id)
    {
        var unit = await context.ProductUnits.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new Exception("Không tìm thấy đơn vị tính.");
        return new ProductUnitDto { Id = unit.Id, Name = unit.Name, IsActive = unit.IsActive };
    }

    public async Task CreateProductUnitAsync(CreateProductUnitDto dto)
    {
        if (await context.ProductUnits.AnyAsync(u => u.Name.ToLower() == dto.Name.ToLower()))
            throw new Exception("Tên đơn vị tính đã tồn tại.");

        context.ProductUnits.Add(new ProductUnit { Name = dto.Name, IsActive = true });
        await context.SaveChangesAsync();
    }

    public async Task UpdateProductUnitAsync(int id, UpdateProductUnitDto dto)
    {
        var unit = await context.ProductUnits.FindAsync(id) ?? throw new Exception("Không tìm thấy đơn vị tính.");
        if (unit.Name.ToLower() != dto.Name.ToLower() && await context.ProductUnits.AnyAsync(u => u.Name.ToLower() == dto.Name.ToLower()))
            throw new Exception("Tên đơn vị tính đã tồn tại.");

        unit.Name = dto.Name;
        unit.IsActive = dto.IsActive;
        await context.SaveChangesAsync();
    }

    public async Task ToggleProductUnitActiveAsync(int id)
    {
        var unit = await context.ProductUnits.FindAsync(id)
            ?? throw new Exception("Không tìm thấy đơn vị tính.");
        unit.IsActive = !unit.IsActive;
        await context.SaveChangesAsync();
    }

    public async Task DeleteProductUnitAsync(int id)
    {
        var unit = await context.ProductUnits.FindAsync(id) ?? throw new Exception("Không tìm thấy đơn vị tính.");
        if (await context.Products.AnyAsync(p => p.ProductUnitId == id))
            throw new Exception("Không thể xóa do đang có sản phẩm sử dụng đơn vị này.");

        context.ProductUnits.Remove(unit);
        await context.SaveChangesAsync();
    }
}