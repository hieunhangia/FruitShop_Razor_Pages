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

    public async Task<List<CartItemDto>> ToCartItemDtoListAsync(List<CartItem> cartItems,
        Func<string, bool, Task<string>> getImageFileUrl)
    {
        var dtos = ToCartItemDtoListBasic(cartItems);
        var tasks = cartItems.Select(item => getImageFileUrl(item.Product!.ImageFilePath, true));
        var imageUrls = await Task.WhenAll(tasks);
        for (var i = 0; i < cartItems.Count; i++)
        {
            dtos[i].ProductImageFileUrl = imageUrls[i];
        }

        return dtos;
    }
}