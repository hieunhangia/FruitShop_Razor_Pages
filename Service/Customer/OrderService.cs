using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Models.Orders;
using Service.DTOs.Customer.Order;

namespace Service.Customer;

public class OrderService(AppDbContext context, CartService cartService)
{
    public async Task CreateCashOnDeliveryOrderAsync(int customerId, CreateOrderDto createOrderDto)
    {
        var shippingAddress = await context.ShippingAddresses.AsNoTracking()
            .Include(sa => sa.Commune)
            .ThenInclude(c => c!.Province)
            .FirstOrDefaultAsync(sa =>
                sa.Id == createOrderDto.ShippingAddressId && sa.CustomerId == customerId);

        if (shippingAddress == null)
        {
            throw new Exception("Địa chỉ giao hàng không tồn tại hoặc không thuộc về khách hàng.");
        }

        var selectedCartItems = await cartService.GetSelectedCartItemsAsync(customerId);

        if (selectedCartItems.CartItems.Count == 0)
        {
            throw new Exception("Vui lòng chọn ít nhất một sản phẩm trong giỏ hàng để thanh toán.");
        }

        if (selectedCartItems.HasUpdates)
        {
            throw new Exception(
                "Một số sản phẩm được chọn thanh toán đã được cập nhật do thay đổi về tình trạng tồn kho. Vui lòng kiểm tra lại giỏ hàng của bạn.");
        }

        var cartItemProductIds = selectedCartItems.CartItems.Select(ci => ci.ProductId).ToList();

        var products = await context.Products.AsNoTracking()
            .Include(p => p.ProductUnit)
            .Where(p => cartItemProductIds.Contains(p.Id))
            .ToListAsync();

        var orderItems = selectedCartItems.CartItems.Select(ci =>
        {
            var product = products.FirstOrDefault(p => p.Id == ci.ProductId);
            if (product is not { IsActive: true })
            {
                throw new Exception($"Sản phẩm {ci.ProductName} không tồn tại hoặc đã ngừng kinh doanh.");
            }

            return new OrderItem
            {
                ProductId = ci.ProductId,
                ProductSnapshot = ProductSnapshot.FromProduct(product).ToJson(),
                Quantity = ci.Quantity,
                UnitPrice = product.Price
            };
        }).ToList();

        await RemoveOrderItemFromCartAsync(customerId, cartItemProductIds);

        context.Orders.Add(new Order
        {
            OrderDate = createOrderDto.OrderDate,
            Status = OrderStatus.PendingConfirmation,
            PaymentMethod = PaymentMethod.CashOnDelivery,
            ShippingAddressSnapshot = ShippingAddressSnapshot.FromShippingAddress(shippingAddress).ToJson(),
            CustomerId = customerId,
            OrderItems = orderItems
        });
        await context.SaveChangesAsync();
    }

    public async Task CreateQRCodePaymentOrderAsync(int customerId, CreateOrderDto createOrderDto)
    {
        throw new NotImplementedException();
    }

    private async Task RemoveOrderItemFromCartAsync(int customerId, List<int> cartItemProductIds)
    {
        var cartItems = await context.CartItems
            .Where(ci => ci.CustomerId == customerId && cartItemProductIds.Contains(ci.ProductId))
            .ToListAsync();
        context.CartItems.RemoveRange(cartItems);
    }
}