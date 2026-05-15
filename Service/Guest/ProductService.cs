using System.Globalization;
using System.Linq.Expressions;
using System.Text;
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
    public async Task<List<ProductDto>> SearchProductsAsync(string searchTerm, int maxResults = BusinessRuleConstants.Homepage.MaxResultSearch)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return [];
        }
        searchTerm = searchTerm.Trim();
        var products = await context.Products
            .AsNoTracking()
            .Include(p => p.ProductUnit)
            .Where(p => p.IsActive && EF.Functions.ILike(p.Name, $"%{searchTerm}%"))
            .OrderBy(p => p.Name)
            .Take(maxResults)
            .ToListAsync();

        return homepageMapper.ToHomepageProductDtoList(products);
    }
    
    public static string RemoveDiacritics(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        string normalizedString = str.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .Replace('đ', 'd')
            .Replace('Đ', 'D');
    }
}
