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

        return await SyncCartWithInventoryAsync(cart);
    }

    public async Task<CartDto> GetSelectedCartItemsAsync(int customerId)
    {
        var cart = await context.CartItems
            .Include(ci => ci.Product)
            .ThenInclude(p => p!.ProductUnit)
            .Where(ci => ci.CustomerId == customerId && ci.IsSelected)
            .ToListAsync();

        return await SyncCartWithInventoryAsync(cart);
    }

    private async Task<CartDto> SyncCartWithInventoryAsync(List<CartItem> cart)
    {
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
        var rowsAffected = await context.CartItems
            .Where(ci => ci.CustomerId == customerId && ci.ProductId == productId)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.IsSelected, isSelected));

        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException("Sản phẩm không tồn tại trong giỏ hàng.");
        }
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

    public async Task AddProductToCartAsync(int customerId, int productId, int quantity)
    {
        var existingItem = await context.CartItems.AsNoTracking()
            .FirstOrDefaultAsync(ci => ci.CustomerId == customerId && ci.ProductId == productId);
        await UpdateCartItemQuantityAsync(customerId, productId, existingItem?.Quantity + quantity ?? quantity);
    }

    public async Task SelectCartItemForBuyNowAsync(int customerId, int productId, int quantity)
    {
        var product = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);
        if (product is not { IsActive: true })
        {
            throw new Exception("Sản phẩm không tồn tại hoặc đã ngừng kinh doanh.");
        }

        if (product.Quantity < quantity)
        {
            throw new Exception("Số lượng sản phẩm trong kho không đủ.");
        }

        await context.CartItems
            .Where(ci => ci.CustomerId == customerId && ci.IsSelected)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.IsSelected, false));

        var cartItem =
            await context.CartItems.FirstOrDefaultAsync(ci => ci.CustomerId == customerId && ci.ProductId == productId);
        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            cartItem.IsSelected = true;
        }
        else
        {
            cartItem = new CartItem
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = quantity,
                IsSelected = true
            };
            context.CartItems.Add(cartItem);
        }

        await context.SaveChangesAsync();
    }

    public async Task<int> CountCartItemsAsync(int customerId) =>
        await context.CartItems.Where(ci => ci.CustomerId == customerId).CountAsync();
}