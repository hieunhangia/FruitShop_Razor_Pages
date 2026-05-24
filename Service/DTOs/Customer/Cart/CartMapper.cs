using Repository.Models.Orders;
using Repository.Models.Products;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Cart;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class CartMapper
{
    [MapProperty($"{nameof(CartItem.Product)}.{nameof(Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(CartItemDto.ProductUnitName))]
    [MapperIgnoreTarget(nameof(CartItemDto.ProductImageFileUrl))]
    private static partial CartItemDto ToCartItemDto(CartItem cartItem);

    public static partial IQueryable<CartItemDto> ProjectToCartItemDto(this IQueryable<CartItem> cartItem);
}