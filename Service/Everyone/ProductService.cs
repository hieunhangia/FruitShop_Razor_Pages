using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Orders;
using Repository.Models.Products;
using Service.DTOs;
using Service.DTOs.Everyone.Product;

namespace Service.Everyone;

public class ProductService(AppDbContext context, FileService fileService)
{
    public async Task<List<ProductSummaryDto>> GetTopProductsAsync(int numberOfTopProduct)
    {
        var products = await context.Products
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .Take(numberOfTopProduct)
            .ProjectToProductSummaryDto()
            .ToListAsync();
        await MapAdditionalProductInfoAsync(products);
        return products;
    }

    public async Task<List<ProductSummaryDto>> GetBestSellingProductsAsync(int numberOfBestSellingProduct)
    {
        var products = await context.Products
            .Where(p => p.IsActive && p.OrderItems!.Any(oi => oi.Order!.OrderStatus == OrderStatus.Delivered))
            .OrderByDescending(p =>
                p.OrderItems!.Where(oi => oi.Order!.OrderStatus == OrderStatus.Delivered).Sum(oi => oi.Quantity))
            .Take(numberOfBestSellingProduct)
            .ProjectToProductSummaryDto()
            .ToListAsync();
        await MapAdditionalProductInfoAsync(products);
        return products;
    }

    public async Task<List<ProductSummaryDto>> GetBestRatingProductsAsync(int numberOfBestRatingProduct)
    {
        var products = await context.Products
            .Where(p => p.IsActive && p.ProductReviews!.Any())
            .OrderByDescending(p => p.ProductReviews!.Average(r => r.Rating))
            .Take(numberOfBestRatingProduct)
            .ProjectToProductSummaryDto()
            .ToListAsync();
        await MapAdditionalProductInfoAsync(products);
        return products;
    }

    public async Task<List<ProductReviewDto>> GetTopProductReviewsAsync(int topProductReviewCount)
    {
        var filteredReviews = context.ProductReviews
            .Where(pr => pr.Rating == BusinessRuleConstants.Model.ProductReview.RatingMaxValue &&
                         pr.CommentClassification == CommentClassification.Positive);

        var newestReviewsQuery = filteredReviews
            .GroupBy(pr => pr.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                NewestCreatedAt = g.Max(x => x.CreatedAt)
            });

        return await filteredReviews
            .Join(newestReviewsQuery,
                pr => new { pr.ProductId, NewestCreatedAt = pr.CreatedAt },
                grp => new { grp.ProductId, grp.NewestCreatedAt },
                (pr, grp) => pr)
            .OrderByDescending(pr => pr.CreatedAt)
            .Take(topProductReviewCount)
            .ProjectToProductReviewDto()
            .ToListAsync();
    }

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
            case "BestRating":
                sortColumn = "ProductReviews.Any() ? ProductReviews.Average(Rating) : 0.0";
                break;
            case "BestSelling":
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
        await MapAdditionalProductInfoAsync(productDtos);
        return new PagedAndSortedDto<ProductSummaryDto>(productDtos, count, request.PageIndex, request.PageSize,
            request.SortColumn ?? nameof(Product.DisplayOrder), request.SortDirection.Value);
    }

    public async Task<ProductDetailDto?> GetProductDetailAsync(int id, int? customerId, int topProductReviewCount)
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
        product.SoldQuantity = await context.OrderItems
            .Where(oi => oi.ProductId == id && oi.Order!.OrderStatus == OrderStatus.Delivered)
            .SumAsync(oi => oi.Quantity);

        if (customerId.HasValue)
        {
            product.QuantityInCart = await context.CartItems
                .Where(ci => ci.CustomerId == customerId && ci.ProductId == id)
                .Select(ci => ci.Quantity).FirstOrDefaultAsync();
        }

        product.TopProductReviews = await context.ProductReviews
            .Where(pr => pr.ProductId == id)
            .OrderByDescending(pr => pr.Rating)
            .ThenBy(pr => pr.CreatedAt)
            .Take(topProductReviewCount)
            .ProjectToProductReviewDto()
            .ToListAsync();

        return product;
    }

    public async Task<ProductInReviewPageDto?> GetProductInReviewPageAsync(int id,
        PagedAndSortedRequest<ProductReviewFilter> request)
    {
        var product = await context.Products
            .Where(p => p.Id == id)
            .ProjectToProductInReviewPageDto()
            .AsSplitQuery()
            .FirstOrDefaultAsync();

        if (product == null)
        {
            return null;
        }

        product.ImageFileUrl = fileService.GetPublicFileUrl(product.ImageFilePath);

        var productReviewsQuery = context.ProductReviews.Where(pr => pr.ProductId == id);

        if (request.Filter.Rating.HasValue)
        {
            productReviewsQuery = productReviewsQuery.Where(pr => pr.Rating == request.Filter.Rating);
        }

        request.SortColumn ??= nameof(ProductReview.CreatedAt);
        request.SortDirection ??= SortDirection.Descending;

        var productReviewsCount = await productReviewsQuery.CountAsync();
        var productReviews = productReviewsCount == 0
            ? []
            : await productReviewsQuery
                .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
                .ThenByDescending(pr => pr.CreatedAt)
                .ApplyPaging(request.PageIndex, request.PageSize)
                .ProjectToProductReviewDto()
                .ToListAsync();

        product.ProductReviews = new PagedAndSortedDto<ProductReviewDto>(productReviews, productReviewsCount,
            request.PageIndex, request.PageSize, request.SortColumn, request.SortDirection.Value);

        return product;
    }

    private async Task MapAdditionalProductInfoAsync(List<ProductSummaryDto> productSummaryDtos)
    {
        var productIds = productSummaryDtos.Select(p => p.Id);
        var soldQuantitiesDictionary = await context.OrderItems
            .Where(oi => productIds.Contains(oi.ProductId) && oi.Order!.OrderStatus == OrderStatus.Delivered)
            .GroupBy(oi => oi.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalSold = g.Sum(oi => oi.Quantity)
            })
            .ToDictionaryAsync(x => x.ProductId, x => x.TotalSold);

        foreach (var product in productSummaryDtos)
        {
            product.ImageFileUrl = fileService.GetPublicFileUrl(product.ImageFilePath);
            product.SoldQuantity = soldQuantitiesDictionary.GetValueOrDefault(product.Id, 0);
        }
    }
}