using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Products;
using Service.DTOs;
using Service.DTOs.Everyone.Product;

namespace Service.Everyone;

public class ProductService(AppDbContext context, FileService fileService)
{
    public async Task<PagedAndSortedDto<ProductSummaryDto>> SearchProductsAsync(
        PagedAndSortedRequest<ProductFilter> request)
    {
        var query = context.Products.Where(p => p.IsActive);

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
            query = query.Where(p =>
                p.ProductReviews!.Average(r => (double?)r.Rating) > minRating &&
                p.ProductReviews!.Average(r => (double?)r.Rating) <= maxRating);
        }

        var sortColumn = request.SortColumn ?? nameof(Product.DisplayOrder);
        request.SortDirection ??= SortDirection.Ascending;

        var count = await query.CountAsync();
        if (count == 0)
        {
            return new PagedAndSortedDto<ProductSummaryDto>([], 0, request.PageIndex, request.PageSize,
                sortColumn, request.SortDirection.Value);
        }

        var orderByParameter = new object();
        switch (sortColumn)
        {
            case "Rating":
                sortColumn = "ProductReviews.Any() ? ProductReviews.Average(Rating) : 0.0";
                break;
            case "BestSeller":
                sortColumn =
                    "OrderItems.Any(Order.OrderStatus == @0) ? OrderItems.Where(Order.OrderStatus == @0).Sum(Quantity) : 0";
                orderByParameter = OrderStatus.Delivered;
                break;
        }

        var productDtos = await query
            .DynamicOrderBy(sortColumn, request.SortDirection.Value, orderByParameter)
            .ThenBy(p => p.DisplayOrder)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ProjectToProductSummaryDto()
            .ToListAsync();

        foreach (var product in productDtos)
        {
            product.ImageFileUrl = fileService.GetPublicFileUrl(product.ImageFilePath);
        }

        return new PagedAndSortedDto<ProductSummaryDto>(productDtos, count, request.PageIndex, request.PageSize,
            request.SortColumn ?? nameof(Product.DisplayOrder), request.SortDirection.Value);
    }

    public async Task<ProductDetailDto?> GetProductDetailAsync(int id, int topProductReviewCount)
    {
        var product = await context.Products
            .Where(p => p.Id == id)
            .ProjectToProductDetailDto()
            .AsSplitQuery()
            .FirstOrDefaultAsync();

        if (product == null)
        {
            return null;
        }

        product.ImageFileUrl = fileService.GetPublicFileUrl(product.ImageFilePath);
        product.TopProductReviews = await context.ProductReviews
            .Where(pr => pr.ProductId == id)
            .OrderByDescending(pr => pr.Rating)
            .ThenBy(pr => pr.CreatedAt)
            .Take(topProductReviewCount)
            .ProjectToProductReviewDto()
            .ToListAsync();

        return product;
    }
}