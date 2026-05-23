using Repository.Models.Products;
using Riok.Mapperly.Abstractions;
using Service.DTOs.Everyone.ProductReview;

namespace Service.DTOs.Everyone.Product;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductMapper(FileService fileService, ProductReviewMapper productReviewMapper)
{
    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductSummaryDto.ProductUnitName))]
    [MapProperty(nameof(Repository.Models.Products.Product.ImageFilePath), nameof(ProductSummaryDto.ImageUrl),
        Use = nameof(MapImageFilePath))]
    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductReviews)}",
        nameof(ProductSummaryDto.AverageRating), Use = nameof(MapAverageRating))]
    public partial ProductSummaryDto ToProductSummaryDto(Repository.Models.Products.Product product);

    [UserMapping(Default = false)]
    private static double? MapAverageRating(ICollection<Repository.Models.Orders.ProductReview> reviews)
    {
        if (reviews.Count == 0)
        {
            return null;
        }

        return Math.Round(reviews.Average(r => r.Rating), 1);
    }

    public partial List<ProductSummaryDto> ToProductSummaryDtoList(List<Repository.Models.Products.Product> products);


    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductDetailDto.ProductUnitName))]
    [MapProperty(nameof(Repository.Models.Products.Product.ImageFilePath), nameof(ProductDetailDto.ImageUrl),
        Use = nameof(MapImageFilePath))]
    [MapperIgnoreTarget(nameof(ProductDetailDto.TopProductReviews))]
    [MapperIgnoreTarget(nameof(ProductDetailDto.ProductReviewCount))]
    [MapperIgnoreTarget(nameof(ProductDetailDto.AverageRating))]
    private partial ProductDetailDto ToProductDetailDtoBasic(Repository.Models.Products.Product product);


    public ProductDetailDto ToProductDetailDto(Repository.Models.Products.Product product,
        List<Repository.Models.Orders.ProductReview> topProductReviews, int productReviewCount, double? averageRating)
    {
        var productDetailDto = ToProductDetailDtoBasic(product);
        productDetailDto.TopProductReviews = productReviewMapper.ToProductReviewDtoList(topProductReviews);
        productDetailDto.ProductReviewCount = productReviewCount;
        productDetailDto.AverageRating = averageRating;
        return productDetailDto;
    }

    [UserMapping(Default = false)]
    private string MapImageFilePath(string imageFilePath) => fileService.GetPublicFileUrl(imageFilePath);
}