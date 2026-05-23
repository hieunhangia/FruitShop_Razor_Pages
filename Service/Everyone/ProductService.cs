using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Products;
using Service.DTOs;
using Service.DTOs.Everyone.Product;

namespace Service.Everyone;

public class ProductService(AppDbContext context, ProductMapper productMapper)
{
    public async Task<PagedAndSortedDto<ProductSummaryDto>> SearchProductsAsync(
        PagedAndSortedRequest<ProductFilter> request)
    {
        var query = context.Products.AsNoTracking().Where(p => p.IsActive);

        var searchTerm = request.Filter.SearchTerm?.Trim();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.WhereAnyContainsUnaccent(searchTerm, p => p.Name, p => p.Description);
        }

        if (request.Filter.MinPrice != null)
        {
            query = query.Where(p => p.Price >= request.Filter.MinPrice);
        }

        if (request.Filter.MaxPrice != null)
        {
            query = query.Where(p => p.Price <= request.Filter.MaxPrice);
        }

        if (request.Filter.CategoryId != null)
        {
            query = query.Where(p => p.Categories!.Any(c => c.Id == request.Filter.CategoryId));
        }

        if (request.Filter.MinAverageRating != null)
        {
            query = query.Where(p =>
                p.ProductReviews!.Any() &&
                p.ProductReviews!.Average(r => r.Rating) >= request.Filter.MinAverageRating);
        }

        if (request.Filter.MaxAverageRating != null)
        {
            query = query.Where(p =>
                p.ProductReviews!.Any() &&
                p.ProductReviews!.Average(r => r.Rating) <= request.Filter.MaxAverageRating);
        }

        request.SortColumn ??= nameof(Product.DisplayOrder);
        request.SortDirection ??= SortDirection.Ascending;

        var count = await query.CountAsync();
        if (count == 0)
        {
            return new PagedAndSortedDto<ProductSummaryDto>([], 0, request.PageIndex, request.PageSize,
                request.SortColumn, request.SortDirection.Value);
        }

        var products = await query
            .Include(p => p.ProductUnit)
            .Include(p => p.Categories!.Where(pc => pc.IsActive))
            .Include(p => p.ProductReviews)
            .AsSplitQuery()
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ToListAsync();

        var productDtos = products.Select(productMapper.ToProductSummaryDto).ToList();

        return new PagedAndSortedDto<ProductSummaryDto>(productDtos, count, request.PageIndex, request.PageSize,
            request.SortColumn, request.SortDirection.Value);
    }

    public async Task<ProductDetailDto?> GetProductDetailAsync(int id, int topProductReviewCount)
    {
        var product = await context.Products
            .AsNoTracking()
            .Include(p => p.ProductUnit)
            .Include(p => p.Categories!.Where(pc => pc.IsActive))
            .Include(p => p.ProductReviews!.OrderByDescending(r => r.CreatedAt).Take(topProductReviewCount))
            .ThenInclude(pr => pr.Customer)
            .ThenInclude(cd => cd!.Customer)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id);

        var productReviewCount = await context.ProductReviews.CountAsync(p => p.ProductId == id);
        double? averageRating = productReviewCount != 0
            ? await context.ProductReviews.Where(p => p.ProductId == id).AverageAsync(p => p.Rating)
            : null;
        return product != null
            ? productMapper.ToProductDetailDto(product, product.ProductReviews!.ToList(), productReviewCount,
                averageRating)
            : null;
    }
}