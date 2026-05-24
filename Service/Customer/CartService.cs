using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Models.Orders;
using Service.DTOs.Customer.Cart;

namespace Service.Customer;

public class CartService(AppDbContext context, FileService fileService)
{
    public async Task<List<CartItemDto>> GetCartAsync(int customerId)
    {
        var cart = await context.CartItems
            .Where(ci => ci.CustomerId == customerId)
            .ProjectToCartItemDto()
            .ToListAsync();

        foreach (var item in cart)
        {
            item.ProductImageFileUrl = fileService.GetPublicFileUrl(item.ProductImageFilePath);
        }

        return cart;
    }

    public async Task<List<CartItemDto>> GetSelectedCartItemsAsync(int customerId)
    {
        var cart = await context.CartItems
            .Where(ci => ci.CustomerId == customerId && ci.IsSelected)
            .ProjectToCartItemDto()
            .ToListAsync();

        foreach (var item in cart)
        {
            item.ProductImageFileUrl = fileService.GetPublicFileUrl(item.ProductImageFilePath);
        }

        return cart;
    }

    public async Task<int> SyncCartWithInventoryAsync(int customerId, List<int> productIds)
    {
        var cart = await context.CartItems
            .Include(cartItem => cartItem.Product)
            .Where(ci => ci.CustomerId == customerId && productIds.Contains(ci.ProductId))
            .ToListAsync();

        foreach (var item in cart)
        {
            if (item.Product is not { IsActive: true } || item.Product.Quantity == 0)
            {
                context.CartItems.Remove(item);
                continue;
            }

            if (item.Product.Quantity < item.Quantity)
            {
                item.Quantity = item.Product.Quantity;
            }
        }

        return await context.SaveChangesAsync();
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