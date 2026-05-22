using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs.Everyone.Product;

namespace Service.Everyone;

public class ProductService(AppDbContext context, ProductMapper productMapper)
{
    public async Task<ProductDetailDto?> GetProductDetailAsync(int id)
    {
        var product = await context.Products
            .AsNoTracking()
            .Include(p => p.ProductUnit)
            .Include(p => p.Categories!.Where(pc => pc.IsActive))
            .Include(p => p.ProductReviews!.OrderByDescending(r => r.CreatedAt))
            .ThenInclude(pr => pr.Customer)
            .ThenInclude(cd => cd!.Customer)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id);

        return product != null ? productMapper.ToProductDetailDto(product) : null;
    }
}