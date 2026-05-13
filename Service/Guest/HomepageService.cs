using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs.Guest;
using Service.DTOs.Guest.Homepage;
using Repository.Constants;

namespace Service.Guest;
public class HomepageService(AppDbContext context, HomepageMapper mapper)
{
    public async Task<List<ProductDto>> GetNewestProductsAsync(int count = HomepageConstant.HomepageProductCount)
    {
        var products = await context.Products
            .AsNoTracking()
            .Include(p => p.ProductUnit)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.Id)
            .Take(count)
            .ToListAsync();

        return mapper.ToHomepageProductDtoList(products);
    }

    public async Task<List<ProductDto>> GetProductsByPriceAsync(int count = HomepageConstant.HomepageProductCount)
    {
        var products = await context.Products
            .AsNoTracking()
            .Include(p => p.ProductUnit)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Price)
            .Take(count)
            .ToListAsync();

        return mapper.ToHomepageProductDtoList(products);
    }
}