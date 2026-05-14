using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Models.Products;
using Service.DTOs.Guest.Homepage;
using Repository;
namespace Service.Guest;
public class ProductService(AppDbContext context, HomepageMapper mapper)
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

        return mapper.ToHomepageProductDtoList(products);
    }
}