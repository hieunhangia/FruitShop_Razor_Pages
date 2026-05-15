using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Models.Products;
using Service.DTOs.Guest.Homepage;
using Repository;
using Service.DTOs.Guest.DetailProduct;

namespace Service.Guest;
public class ProductService(AppDbContext context, HomepageMapper homepageMapper, ProductDetailMapper productDetailMapper)
{
    public async Task<List<ProductDto>> GetNewestProductsAsync(int count = BusinessRuleConstants.Homepage.ProductsCount)
    {
        return await GetProducts(p => p.IsActive, p => p.Id,count);
    }

    public async Task<List<ProductDto>> GetProductsByPriceAsync(int count = BusinessRuleConstants.Homepage.ProductsCount)
    {
        return await GetProducts(p => p.IsActive, p => p.Price,count);
    }

    public async Task<List<ProductDto>> GetProducts(
        Expression<Func<Product, bool>>? filter = null, 
        Expression<Func<Product, object>>? order = null, 
        int count = BusinessRuleConstants.Homepage.ProductsCount)
    {
        IQueryable<Product> query = context.Products
            .AsNoTracking()
            .Include(p => p.ProductUnit);

        if (filter != null)
        {
            query = query.Where(filter);
        }
        query = query.OrderBy(order ?? (p => p.DisplayOrder));
        
        var products = await query.Take(count).ToListAsync();

        return homepageMapper.ToHomepageProductDtoList(products);
    }

    public async Task<ProductDetailDto?> GetProductDetailByIdAsync(int id)
    {
        var product = await context.Products
            .AsNoTracking()
            .Include(p => p.ProductUnit)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) 
        {
            return null;
        }

        return productDetailMapper.ToProductDetailDto(product);
    }
}