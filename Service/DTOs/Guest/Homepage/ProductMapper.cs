namespace Service.DTOs.Guest.Homepage;
using Repository.Models.Products;
using Riok.Mapperly.Abstractions;


[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class HomepageMapper
{
    [MapProperty($"{nameof(Product.ProductUnit)}.{nameof(ProductUnit.Name)}", nameof(ProductDto.ProductUnitName))]
    public partial ProductDto ToHomepageProductDto(Product product);

    public partial List<ProductDto> ToHomepageProductDtoList(List<Product> products);
}