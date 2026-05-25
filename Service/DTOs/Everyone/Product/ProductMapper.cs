using Repository;
using Repository.Models.Products;
using Repository.Models.Users;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Everyone.Product;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ProductMapper
{
    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductSummaryDto.ProductUnitName))]
    [MapProperty(nameof(Repository.Models.Products.Product.ProductReviews), nameof(ProductSummaryDto.AverageRating),
        Use = nameof(MapAverageRating))]
    [MapperIgnoreTarget(nameof(ProductSummaryDto.ImageFileUrl))]
    private static partial ProductSummaryDto ToProductSummaryDto(Repository.Models.Products.Product product);

    public static partial IQueryable<ProductSummaryDto> ProjectToProductSummaryDto(
        this IQueryable<Repository.Models.Products.Product> product);

    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductDetailDto.ProductUnitName))]
    [MapProperty(nameof(Repository.Models.Products.Product.ProductReviews), nameof(ProductDetailDto.AverageRating),
        Use = nameof(MapAverageRating))]
    [MapperIgnoreTarget(nameof(ProductDetailDto.ImageFileUrl))]
    [MapperIgnoreTarget(nameof(ProductDetailDto.TopProductReviews))]
    private static partial ProductDetailDto ToProductDetailDto(Repository.Models.Products.Product product);

    public static partial IQueryable<ProductDetailDto> ProjectToProductDetailDto(
        this IQueryable<Repository.Models.Products.Product> product);

    [MapProperty(
        $"{nameof(Repository.Models.Orders.ProductReview.Customer)}.{nameof(CustomerData.Customer)}.{nameof(User.Email)}",
        nameof(ProductReviewDto.ReviewerEmail), Use = nameof(MapReviewerEmail))]
    private static partial ProductReviewDto ToProductReviewDto(Repository.Models.Orders.ProductReview productReview);

    public static partial IQueryable<ProductReviewDto> ProjectToProductReviewDto(
        this IQueryable<Repository.Models.Orders.ProductReview> productReview);

    [UserMapping(Default = false)]
    private static double? MapAverageRating(ICollection<Repository.Models.Orders.ProductReview>? productReviews) =>
        productReviews?.Average(x => (double?)x.Rating);

    [UserMapping(Default = false)]
    private static string MapReviewerEmail(string email) =>
        BusinessRuleConstants.ProductReview.HideEmailAddress(email);
}