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
    [MapperIgnoreTarget(nameof(ProductSummaryDto.AverageRating))]
    private partial ProductSummaryDto ToProductSummaryDtoBasic(Repository.Models.Products.Product product);

    public ProductSummaryDto ToProductSummaryDto(Repository.Models.Products.Product product, double? averageRating)
    {
        var productSummaryDto = ToProductSummaryDtoBasic(product);
        productSummaryDto.AverageRating = averageRating;
        return productSummaryDto;
    }

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