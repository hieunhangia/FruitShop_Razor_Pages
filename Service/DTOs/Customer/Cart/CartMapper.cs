using Repository.Models.Orders;
using Repository.Models.Products;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Cart;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class CartMapper
{
    [MapProperty($"{nameof(CartItem.Product)}.{nameof(Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(CartItemDto.ProductUnitName))]
    public partial CartItemDto ToCartItemDto(CartItem cartItem);

    public partial List<CartItemDto> ToCartItemDtoList(List<CartItem> cartItems);
}