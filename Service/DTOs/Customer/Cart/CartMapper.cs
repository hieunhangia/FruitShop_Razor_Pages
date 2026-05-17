using Repository.Models.Orders;
using Repository.Models.Products;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Cart;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CartMapper(FileService fileService)
{
    [MapProperty($"{nameof(CartItem.Product)}.{nameof(Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(CartItemDto.ProductUnitName))]
    [MapProperty($"{nameof(CartItem.Product)}.{nameof(Product.ImageFilePath)}", nameof(CartItemDto.ProductImageFileUrl),
        Use = nameof(MapProductImageFilePath))]
    public partial CartItemDto ToCartItemDto(CartItem cartItem);

    [UserMapping(Default = false)]
    private string MapProductImageFilePath(string imageFilePath) => fileService.GetPublicFileUrl(imageFilePath);

    public partial List<CartItemDto> ToCartItemDtoList(List<CartItem> cartItems);
}