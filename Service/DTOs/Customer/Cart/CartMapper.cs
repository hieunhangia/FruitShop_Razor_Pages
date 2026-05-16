using Repository.Models.Orders;
using Repository.Models.Products;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Cart;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CartMapper
{
    [MapProperty($"{nameof(CartItem.Product)}.{nameof(Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(CartItemDto.ProductUnitName))]
    [MapperIgnoreTarget(nameof(CartItemDto.ProductImageFileUrl))]
    private partial CartItemDto ToCartItemDtoBasic(CartItem cartItem);

    private partial List<CartItemDto> ToCartItemDtoListBasic(List<CartItem> cartItems);

    public List<CartItemDto> ToCartItemDtoList(List<CartItem> cartItems, Func<string, string> getImageFileUrl)
    {
        var dtos = ToCartItemDtoListBasic(cartItems);
        for (var i = 0; i < cartItems.Count; i++)
        {
            dtos[i].ProductImageFileUrl = getImageFileUrl(cartItems[i].Product!.ImageFilePath);
        }

        return dtos;
    }
}