using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs.Everyone.Product;

namespace Service.Everyone;

public class ProductService(AppDbContext context, ProductMapper productMapper)
{
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

        var dto = productMapper.ToProductDetailDto(product);
        return dto;
    }


    public static string RemoveDiacritics(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        var normalizedString = str.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
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