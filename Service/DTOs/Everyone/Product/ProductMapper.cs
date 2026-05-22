using Repository.Models.Products;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Everyone.Product;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductMapper(FileService fileService)
{
    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductSummaryDto.ProductUnitName))]
    [MapProperty(nameof(Repository.Models.Products.Product.ImageFilePath), nameof(ProductSummaryDto.ImageUrl),
        Use = nameof(MapImageFilePath))]
    public partial ProductSummaryDto ToProductSummaryDto(Repository.Models.Products.Product product);

    public partial List<ProductSummaryDto> ToProductSummaryDtoList(List<Repository.Models.Products.Product> products);


    public partial ProductReviewDto ToProductReviewDto(Repository.Models.Orders.ProductReview productReview);

    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductDetailDto.ProductUnitName))]
    [MapProperty(nameof(Repository.Models.Products.Product.ImageFilePath), nameof(ProductDetailDto.ImageUrl),
        Use = nameof(MapImageFilePath))]
    public partial ProductDetailDto ToProductDetailDto(Repository.Models.Products.Product product);


    [UserMapping(Default = false)]
    private string MapImageFilePath(string imageFilePath) => fileService.GetPublicFileUrl(imageFilePath);
}