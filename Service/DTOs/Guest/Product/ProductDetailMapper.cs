namespace Service.DTOs.Guest.DetailProduct;

using Repository.Models.Products;
using Riok.Mapperly.Abstractions;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductDetailMapper
{
    [MapProperty($"{nameof(Product.ProductUnit)}.{nameof(ProductUnit.Name)}", nameof(ProductDetailDto.ProductUnitName))]
    public partial ProductDetailDto ToProductDetailDto(Product product);

    public partial CategoryDto ToCategoryDto(Category category);
}