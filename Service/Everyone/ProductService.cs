using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Products;
using Service.DTOs;
using Service.DTOs.Everyone.Product;

namespace Service.Everyone;

public class ProductService(AppDbContext context, ProductMapper mapper)
{
    public async Task<PagedAndSortedDto<ProductSummaryDto>> SearchProductsAsync(
        PagedAndSortedRequest<ProductFilter> request)
    {
        var query = context.Products.AsNoTracking().Where(p => p.IsActive);

        var searchTerm = request.Filter.SearchTerm?.Trim();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.WhereContainsUnaccent(searchTerm, p => p.Name);
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

        if (request.Filter.AverageRating != null)
        {
            double maxRating = request.Filter.AverageRating.Value;
            var minRating = maxRating - 1;
            query = query
                .Select(p => new
                {
                    Product = p,
                    Avg = p.ProductReviews!.Average(r => (double?)r.Rating)
                })
                .Where(x => x.Avg != null && x.Avg > minRating && x.Avg <= maxRating)
                .Select(x => x.Product);
        }

        request.SortColumn ??= nameof(Product.DisplayOrder);
        request.SortDirection ??= SortDirection.Ascending;

        var count = await query.CountAsync();
        if (count == 0)
        {
            return new PagedAndSortedDto<ProductSummaryDto>([], 0, request.PageIndex, request.PageSize,
                request.SortColumn, request.SortDirection.Value);
        }

        var orderByParameter = new object();
        switch (request.SortColumn)
        {
            case "Rating":
                request.SortColumn = "ProductReviews.Any() ? ProductReviews.Average(Rating) : 0.0";
                break;
            case "BestSeller":
                request.SortColumn =
                    "OrderItems.Any(Order.OrderStatus == @0) ? OrderItems.Where(Order.OrderStatus == @0).Sum(Quantity) : 0";
                orderByParameter = OrderStatus.Delivered;
                break;
        }

        var products = await query
            .Include(p => p.ProductUnit)
            .Include(p => p.Categories!.Where(pc => pc.IsActive))
            .Include(p => p.ProductReviews)
            .AsSplitQuery()
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value, orderByParameter)
            .ThenBy(p => p.DisplayOrder)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ToListAsync();

        var productDtos = mapper.ToProductSummaryDtoList(products);

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
        var averageRating = await context.ProductReviews
            .Where(p => p.ProductId == id)
            .AverageAsync(p => (double?)p.Rating);
        return product != null
            ? mapper.ToProductDetailDto(product, product.ProductReviews!.ToList(), productReviewCount, averageRating)
            : null;
    }
}