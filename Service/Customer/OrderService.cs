using Microsoft.EntityFrameworkCore;
using PayOS;
using PayOS.Models.V2.PaymentRequests;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Orders;
using Repository.Models.Products;
using Repository.Models.Users;
using Service.DTOs;
using Service.DTOs.Customer.Order;

namespace Service.Customer;

public class OrderService(
    AppDbContext context,
    OrderMapper mapper,
    CartService cartService,
    PayOSClient payOsClient,
    EmailService emailService)
{
    public async Task CreateCashOnDeliveryOrderAsync(int customerId,
        CreateCashOnDeliveryOrderDto createCashOnDeliveryOrderDto)
    {
        var (shippingAddress, orderItems) =
            await PrepareOrderAsync(customerId, createCashOnDeliveryOrderDto.ShippingAddressId);

        var orderId = BusinessRuleConstants.Order.GenerateUniqueOrderId();
        context.Orders.Add(new Order
        {
            Id = orderId,
            OrderDate = DateTime.UtcNow,
            OrderStatus = OrderStatus.PendingConfirmation,
            PaymentMethod = PaymentMethod.CashOnDelivery,
            TotalAmount = orderItems.Sum(oi => oi.Quantity * oi.ProductSnapshot.UnitPrice),
            ShippingAddressSnapshot = new ShippingAddressSnapshot
            {
                RecipientName = shippingAddress.RecipientName,
                RecipientPhoneNumber = shippingAddress.RecipientPhoneNumber,
                SpecificAddress = shippingAddress.SpecificAddress,
                CommuneName = shippingAddress.Commune!.Name,
                ProvinceName = shippingAddress.Commune.Province!.Name
            },
            CustomerId = customerId,
            OrderItems = orderItems
        });
        await context.SaveChangesAsync();

        _ = emailService.SendEmailAsync(createCashOnDeliveryOrderDto.CustomerEmail, "Xác nhận đặt hàng",
            $"Đơn hàng của bạn đã được tạo thành công với mã đơn hàng: {orderId}. Vui lòng chờ xác nhận từ cửa hàng. Cảm ơn bạn đã mua sắm tại FruitShop!");
    }

    public async Task<string> CreateQRCodePaymentOrderAsync(int customerId,
        CreateQrCodePaymentDto createQrCodePaymentDto)
    {
        var (shippingAddress, orderItems) =
            await PrepareOrderAsync(customerId, createQrCodePaymentDto.ShippingAddressId);

        var orderId = BusinessRuleConstants.Order.GenerateUniqueOrderId();
        var order = new Order
        {
            Id = orderId,
            OrderDate = DateTime.UtcNow,
            OrderStatus = OrderStatus.PendingPayment,
            PaymentMethod = PaymentMethod.QRCode,
            TotalAmount = orderItems.Sum(oi => oi.Quantity * oi.ProductSnapshot.UnitPrice),
            ShippingAddressSnapshot = new ShippingAddressSnapshot
            {
                RecipientName = shippingAddress.RecipientName,
                RecipientPhoneNumber = shippingAddress.RecipientPhoneNumber,
                SpecificAddress = shippingAddress.SpecificAddress,
                CommuneName = shippingAddress.Commune!.Name,
                ProvinceName = shippingAddress.Commune.Province!.Name
            },
            CustomerId = customerId,
            OrderItems = orderItems
        };
        var paymentExpirationDate =
            DateTimeOffset.UtcNow.AddMinutes(BusinessRuleConstants.Order.QrCodePaymentOrderExpiredMinutes);
        var paymentRequest = new CreatePaymentLinkRequest
        {
            OrderCode = order.Id,
            Amount = order.TotalAmount,
            ReturnUrl = createQrCodePaymentDto.ReturnUrl,
            CancelUrl = createQrCodePaymentDto.CancelUrl,
            ExpiredAt = paymentExpirationDate.ToUnixTimeSeconds(),
            Description = $"FruitShop {order.Id}",
            Items = orderItems.Select(oi => new PaymentLinkItem
            {
                Name = oi.ProductSnapshot.Name,
                Quantity = oi.Quantity,
                Price = oi.ProductSnapshot.UnitPrice
            }).ToList()
        };
        var paymentResponse = await payOsClient.PaymentRequests.CreateAsync(paymentRequest);
        order.QrCodePaymentData = new OrderQrCodePaymentData
        {
            ExpirationDate = paymentExpirationDate.UtcDateTime,
            PaymentLink = paymentResponse.CheckoutUrl
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        _ = emailService.SendEmailAsync(createQrCodePaymentDto.CustomerEmail, "Xác nhận đặt hàng",
            $"Đơn hàng của bạn đã được tạo thành công với mã đơn hàng: {orderId}. Vui lòng thanh toán trước {paymentExpirationDate:HH:mm dd/MM/yyyy} để đơn hàng được xử lý. Cảm ơn bạn đã mua sắm tại FruitShop!");

        return paymentResponse.CheckoutUrl;
    }

    private async Task<(ShippingAddress shippingAddress, List<OrderItem> orderItems)> PrepareOrderAsync(int customerId,
        int shippingAddressId)
    {
        var shippingAddress = await context.ShippingAddresses.AsNoTracking()
            .Include(sa => sa.Commune)
            .ThenInclude(c => c!.Province)
            .FirstOrDefaultAsync(sa => sa.Id == shippingAddressId && sa.CustomerId == customerId);

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

        var cartItems = await context.CartItems
            .Where(ci => ci.CustomerId == customerId && cartItemProductIds.Contains(ci.ProductId))
            .ToListAsync();
        context.CartItems.RemoveRange(cartItems);

        var products = await context.Products
            .Include(p => p.ProductUnit)
            .Where(p => cartItemProductIds.Contains(p.Id))
            .ToListAsync();
        var orderItems = selectedCartItems.CartItems.Select(ci =>
        {
            var product = products.First(p => p.Id == ci.ProductId);
            HoldProducts(product, ci.Quantity);

            return new OrderItem
            {
                ProductId = ci.ProductId,
                ProductSnapshot = new ProductSnapshot
                {
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    UnitPrice = product.Price,
                    ProductUnitName = product.ProductUnit!.Name
                },
                Quantity = ci.Quantity
            };
        }).ToList();
        return (shippingAddress, orderItems);
    }

    public async Task CancelCashOnDeliveryOrderAsync(int customerId, long orderId)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)!
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == customerId);

        if (order == null)
        {
            throw new Exception("Đơn hàng không tồn tại hoặc không thuộc về khách hàng.");
        }

        if (order.OrderStatus != OrderStatus.PendingConfirmation || order.PaymentMethod != PaymentMethod.CashOnDelivery)
        {
            throw new Exception(
                "Đơn hàng không ở trạng thái chờ xác nhận hoặc không phải là đơn hàng thanh toán khi nhận hàng.");
        }

        order.OrderStatus = OrderStatus.Cancelled;
        foreach (var orderItem in order.OrderItems!)
        {
            ReleaseHeldProducts(orderItem.Product!, orderItem.Quantity);
        }

        await context.SaveChangesAsync();
    }

    public async Task ConfirmQrCodePaymentOrderAsync(long orderId)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)!
            .ThenInclude(oi => oi.Product)
            .Include(order => order.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception("Đơn hàng không tồn tại.");
        }

        if (order.PaymentMethod != PaymentMethod.QRCode)
        {
            throw new Exception("Đơn hàng không phải là đơn hàng thanh toán bằng QR Code.");
        }

        if ((await payOsClient.PaymentRequests.GetAsync(order.Id.ToString())).Status != PaymentLinkStatus.Paid)
        {
            throw new Exception("Không thể xác nhận đơn hàng vì liên kết thanh toán chưa được thanh toán.");
        }

        order.OrderStatus = OrderStatus.Processing;
        foreach (var orderItem in order.OrderItems!)
        {
            FinalizeProducts(orderItem.Product!, orderItem.Quantity);
        }

        await context.SaveChangesAsync();
        _ = emailService.SendEmailAsync(order.Customer!.Email!, "Xác nhận thanh toán thành công",
            $"Thanh toán cho đơn hàng {order.Id} đã được xác nhận thành công. Đơn hàng của bạn đang được xử lý và sẽ sớm được giao đến bạn. Cảm ơn bạn đã mua sắm tại FruitShop!");
    }

    public async Task CancelQrCodePaymentOrderByPayOsCallbackAsync(long orderId)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)!
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception("Đơn hàng không tồn tại.");
        }

        if (order.PaymentMethod != PaymentMethod.QRCode)
        {
            throw new Exception("Đơn hàng không phải là đơn hàng thanh toán bằng QR Code.");
        }

        if ((await payOsClient.PaymentRequests.GetAsync(order.Id.ToString())).Status != PaymentLinkStatus.Cancelled)
        {
            throw new Exception("Không thể hủy đơn hàng vì liên kết thanh toán chưa được hủy.");
        }

        order.OrderStatus = OrderStatus.Cancelled;
        foreach (var orderItem in order.OrderItems!)
        {
            ReleaseHeldProducts(orderItem.Product!, orderItem.Quantity);
        }

        await context.SaveChangesAsync();
    }

    public async Task CancelQrCodePaymentOrderByCustomerAsync(int customerId, long orderId)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)!
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == customerId);

        if (order == null)
        {
            throw new Exception("Đơn hàng không tồn tại hoặc không thuộc về khách hàng.");
        }

        if (order.OrderStatus != OrderStatus.PendingPayment || order.PaymentMethod != PaymentMethod.QRCode)
        {
            throw new Exception("Đơn hàng không ở trạng thái chờ thanh toán bằng QR Code.");
        }

        var paymentLink = await payOsClient.PaymentRequests.CancelAsync(order.Id.ToString());
        if (paymentLink.Status != PaymentLinkStatus.Cancelled)
        {
            throw new Exception("Không thể hủy đơn hàng vì liên kết thanh toán chưa được hủy.");
        }

        order.OrderStatus = OrderStatus.Cancelled;
        foreach (var orderItem in order.OrderItems!)
        {
            ReleaseHeldProducts(orderItem.Product!, orderItem.Quantity);
        }

        await context.SaveChangesAsync();
    }

    public async Task<int> CancelAllExpiredQrCodePaymentOrdersAsync()
    {
        var expiredOrders = await context.Orders
            .Include(o => o.OrderItems)!
            .ThenInclude(oi => oi.Product)
            .Where(o => o.PaymentMethod == PaymentMethod.QRCode
                        && o.OrderStatus == OrderStatus.PendingPayment
                        && o.QrCodePaymentData!.ExpirationDate < DateTime.UtcNow)
            .ToListAsync();

        foreach (var order in expiredOrders)
        {
            order.OrderStatus = OrderStatus.Cancelled;
            foreach (var orderItem in order.OrderItems!)
            {
                ReleaseHeldProducts(orderItem.Product!, orderItem.Quantity);
            }
        }

        await context.SaveChangesAsync();
        return expiredOrders.Count;
    }

    public async Task<PagedAndSortedDto<OrderSummaryDto>> GetOrderHistoryListAsync(int customerId,
        PagedAndSortedRequest<OrderFilter> pagedAndSortedRequest)
    {
        var query = context.Orders.AsNoTracking().Where(o => o.CustomerId == customerId);

        var searchId = pagedAndSortedRequest.Filter.SearchId?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(searchId))
        {
            query = query.Where(o => o.Id.ToString().Contains(searchId));
        }

        var startDateFilter = pagedAndSortedRequest.Filter.StartDate ?? DateTime.MinValue;
        var endDateFilter = pagedAndSortedRequest.Filter.EndDate ?? DateTime.UtcNow;
        query = query.Where(o => o.OrderDate >= startDateFilter && o.OrderDate <= endDateFilter);

        if (pagedAndSortedRequest.Filter.OrderStatus.HasValue)
        {
            query = query.Where(o => o.OrderStatus == pagedAndSortedRequest.Filter.OrderStatus.Value);
        }

        if (pagedAndSortedRequest.Filter.PaymentMethod.HasValue)
        {
            query = query.Where(o => o.PaymentMethod == pagedAndSortedRequest.Filter.PaymentMethod.Value);
        }

        var startTotalAmountFilter = pagedAndSortedRequest.Filter.StartTotalAmount ?? 0;
        var endTotalAmountFilter = pagedAndSortedRequest.Filter.EndTotalAmount ?? long.MaxValue;
        query = query.Where(o => o.TotalAmount >= startTotalAmountFilter && o.TotalAmount <= endTotalAmountFilter);

        pagedAndSortedRequest.SortColumn ??= nameof(Order.OrderDate);
        pagedAndSortedRequest.SortDirection ??= SortDirection.Descending;

        var totalCount = await query.CountAsync();
        if (totalCount == 0)
        {
            return new PagedAndSortedDto<OrderSummaryDto>([], 0, pagedAndSortedRequest.PageIndex,
                pagedAndSortedRequest.PageSize, pagedAndSortedRequest.SortColumn,
                pagedAndSortedRequest.SortDirection.Value);
        }

        var orders = await query
            .Include(o => o.OrderItems)
            .DynamicOrderBy(pagedAndSortedRequest.SortColumn, pagedAndSortedRequest.SortDirection.Value)
            .Skip((pagedAndSortedRequest.PageIndex - 1) * pagedAndSortedRequest.PageSize)
            .Take(pagedAndSortedRequest.PageSize)
            .ToListAsync();
        return new PagedAndSortedDto<OrderSummaryDto>(mapper.ToOrderSummaryDtoList(orders), totalCount,
            pagedAndSortedRequest.PageIndex, pagedAndSortedRequest.PageSize, pagedAndSortedRequest.SortColumn,
            pagedAndSortedRequest.SortDirection.Value);
    }

    public async Task<OrderDetailDto> GetOrderDetailAsync(int customerId, long orderId)
    {
        var order = await context.Orders.AsNoTracking()
            .Include(o => o.Shipper)
            .ThenInclude(s => s!.ShipperInformation)
            .Include(o => o.OrderItems)
            .Include(o => o.QrCodePaymentData)
            .Include(o => o.OrderShippings)
            .AsSplitQuery()
            .FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == customerId);
        order?.OrderShippings = order.OrderShippings!.OrderBy(os => os.OccurredAt).ToList();
        return order == null
            ? throw new Exception("Đơn hàng không tồn tại hoặc không thuộc về khách hàng.")
            : mapper.ToOrderDetailDto(order);
    }

    private static void HoldProducts(Product product, int quantity)
    {
        product.Quantity -= quantity;
        product.HeldQuantity += quantity;
    }

    private static void ReleaseHeldProducts(Product product, int quantity)
    {
        product.Quantity += quantity;
        product.HeldQuantity -= quantity;
    }

    private static void FinalizeProducts(Product product, int quantity)
    {
        product.HeldQuantity -= quantity;
    }
}