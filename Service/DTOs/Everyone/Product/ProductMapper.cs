using Repository.Models.Products;
using Riok.Mapperly.Abstractions;
using Repository;
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

    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductDetailDto.ProductUnitName))]
    [MapProperty(nameof(Repository.Models.Products.Product.ImageFilePath), nameof(ProductDetailDto.ImageUrl),
        Use = nameof(MapImageFilePath))]
    public partial ProductDetailDto ToProductDetailDto(Repository.Models.Products.Product product);

    [UserMapping(Default = false)]
    private string MapImageFilePath(string imageFilePath)
    {
        if (string.IsNullOrWhiteSpace(imageFilePath))
        {
            return string.Empty;
        }
        imageFilePath = fileService.GetPublicFileUrl(imageFilePath);
        return Path.HasExtension(imageFilePath)
            ? imageFilePath
            : imageFilePath + BusinessRuleConstants.FileService.FileExtension;
    }
}