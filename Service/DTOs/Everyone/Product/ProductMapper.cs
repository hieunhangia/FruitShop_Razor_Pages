using Repository.Models.Products;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Everyone.Product;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductMapper
{
    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductSummaryDto.ProductUnitName))]
    public partial ProductSummaryDto ToProductSummaryDto(Repository.Models.Products.Product product);

    public partial List<ProductSummaryDto> ToProductSummaryDtoList(List<Repository.Models.Products.Product> products);

    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductDetailDto.ProductUnitName))]
    public partial ProductDetailDto ToProductDetailDto(Repository.Models.Products.Product product);
}