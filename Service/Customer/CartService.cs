using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Models.Orders;
using Service.DTOs.Customer.Cart;

namespace Service.Customer;

public class CartService(AppDbContext context, CartMapper mapper)
{
    public async Task<CartDto> GetCartAsync(int customerId)
    {
        var cart = await context.CartItems
            .Include(ci => ci.Product)
            .ThenInclude(p => p!.ProductUnit)
            .Where(ci => ci.CustomerId == customerId)
            .ToListAsync();

        var hasUpdates = false;
        for (var i = cart.Count - 1; i >= 0; i--)
        {
            var cartItem = cart[i];
            var product = cartItem.Product;
            if (product is not { IsActive: true } || product.Quantity == 0)
            {
                context.CartItems.Remove(cartItem);
                cart.RemoveAt(i);
                hasUpdates = true;
                continue;
            }

            if (product.Quantity >= cartItem.Quantity) continue;
            cartItem.Quantity = product.Quantity;
            hasUpdates = true;
        }

        if (hasUpdates)
        {
            await context.SaveChangesAsync();
        }

        return new CartDto
        {
            CartItems = mapper.ToCartItemDtoList(cart),
            HasUpdates = hasUpdates
        };
    }

    public async Task<CartDto> GetSelectedCartItemsAsync(int customerId)
    {
        var cart = await context.CartItems
            .Include(ci => ci.Product)
            .ThenInclude(p => p!.ProductUnit)
            .Where(ci => ci.CustomerId == customerId && ci.IsSelected)
            .ToListAsync();

        var hasUpdates = false;
        for (var i = cart.Count - 1; i >= 0; i--)
        {
            var cartItem = cart[i];
            var product = cartItem.Product;
            if (product is not { IsActive: true } || product.Quantity == 0)
            {
                context.CartItems.Remove(cartItem);
                cart.RemoveAt(i);
                hasUpdates = true;
                continue;
            }

            if (product.Quantity >= cartItem.Quantity) continue;
            cartItem.Quantity = product.Quantity;
            hasUpdates = true;
        }

        if (hasUpdates)
        {
            await context.SaveChangesAsync();
        }

        return new CartDto
        {
            CartItems = mapper.ToCartItemDtoList(cart),
            HasUpdates = hasUpdates
        };
    }

    public async Task UpdateCartItemSelectionAsync(int customerId, int productId, bool isSelected)
    {
        var cartItem =
            await context.CartItems.FirstOrDefaultAsync(ci => ci.CustomerId == customerId && ci.ProductId == productId);
        if (cartItem == null)
        {
            throw new Exception("Sản phẩm không tồn tại trong giỏ hàng.");
        }

        cartItem.IsSelected = isSelected;
        await context.SaveChangesAsync();
    }

    public async Task UpdateCartItemQuantityAsync(int customerId, int productId, int quantity)
    {
        switch (quantity)
        {
            case < 0:
                throw new Exception("Số lượng sản phẩm phải lớn hơn hoặc bằng 0.");
            case 0:
                await context.CartItems
                    .Where(ci => ci.CustomerId == customerId && ci.ProductId == productId)
                    .ExecuteDeleteAsync();
                return;
        }

        var product = await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId);
        if (product is not { IsActive: true })
        {
            throw new Exception("Sản phẩm không tồn tại hoặc đã ngừng kinh doanh.");
        }

        var cartItem =
            await context.CartItems.FirstOrDefaultAsync(ci => ci.CustomerId == customerId && ci.ProductId == productId);
        if (product.Quantity < quantity)
        {
            throw new Exception("Số lượng sản phẩm trong kho không đủ.");
        }

        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
        }
        else
        {
            cartItem = new CartItem
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = quantity
            };
            context.CartItems.Add(cartItem);
        }

        await context.SaveChangesAsync();
    }
    
    public async Task<CartItem?> GetCartItem(int customerId, int productId)
    {
        var cartItem =
            await context.CartItems.FirstOrDefaultAsync(ci => ci.CustomerId == customerId && ci.ProductId == productId);
        return cartItem;
    }
    
    public async Task AddQuantityForProductCart(int customerId, int productId, int quantity = BusinessRuleConstants.Order.MinProductQuantity)
    {
        var cartItem = await GetCartItem(customerId, productId);
        if (cartItem == null)
        {
            await UpdateCartItemQuantityAsync(customerId, productId, quantity);
        }
        else
        {
            var quantityToUpdate = cartItem.Quantity + quantity;
            await UpdateCartItemQuantityAsync(customerId, productId, quantityToUpdate);
        }
    }
    
    
    public async Task<int> CountCartItemsAsync(int customerId) =>
        await context.CartItems
            .AsNoTracking()
            .Where(ci => ci.CustomerId == customerId)
            .CountAsync();
}