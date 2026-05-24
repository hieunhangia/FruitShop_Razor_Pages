using Microsoft.EntityFrameworkCore;
using PayOS;
using PayOS.Models.V2.PaymentRequests;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Coupons;
using Repository.Models.Orders;
using Repository.Models.Products;
using Repository.Models.Users;
using Service.DTOs;
using Service.DTOs.Customer.Order;

namespace Service.Customer;

public class OrderService(
    AppDbContext context,
    PayOSClient payOsClient,
    EmailService emailService,
    OrderMapper mapper)
{
    public async Task CreateCashOnDeliveryOrderAsync(int customerId,
        CreateCashOnDeliveryOrderDto createCashOnDeliveryOrderDto)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        var (shippingAddress, totalAmountBeforeDiscount, totalAmount, loyaltyPointsEarned, orderItems) =
            await PrepareOrderAsync(customerId,
                createCashOnDeliveryOrderDto.ShippingAddressId, createCashOnDeliveryOrderDto.CustomerCouponId);
        var orderId = BusinessRuleConstants.Order.GenerateUniqueOrderId();
        context.Orders.Add(new Order
        {
            Id = orderId,
            OrderDate = DateTime.UtcNow,
            OrderStatus = OrderStatus.PendingConfirmation,
            PaymentMethod = PaymentMethod.CashOnDelivery,
            TotalAmountBeforeDiscount = totalAmountBeforeDiscount,
            TotalAmount = totalAmount,
            LoyaltyPointsEarned = loyaltyPointsEarned,
            ShippingAddressSnapshot = new ShippingAddressSnapshot
            {
                RecipientName = shippingAddress.RecipientName,
                RecipientPhoneNumber = shippingAddress.RecipientPhoneNumber,
                SpecificAddress = shippingAddress.SpecificAddress,
                CommuneName = shippingAddress.Commune!.Name,
                ProvinceName = shippingAddress.Commune.Province!.Name
            },
            CustomerCouponId = createCashOnDeliveryOrderDto.CustomerCouponId,
            CustomerId = customerId,
            OrderItems = orderItems
        });
        await context.SaveChangesAsync();

        await transaction.CommitAsync();

        _ = emailService.SendEmailAsync(createCashOnDeliveryOrderDto.CustomerEmail, "Xác nhận đặt hàng",
            $"Đơn hàng của bạn đã được tạo thành công với mã đơn hàng: {orderId}. Vui lòng chờ xác nhận từ cửa hàng. Cảm ơn bạn đã mua sắm tại FruitShop!");
    }

    public async Task<string> CreateQRCodePaymentOrderAsync(int customerId,
        CreateQrCodePaymentDto createQrCodePaymentDto)
    {
        var (shippingAddress, totalAmountBeforeDiscount, totalAmount, loyaltyPointsEarned, orderItems) =
            await PrepareOrderAsync(customerId, createQrCodePaymentDto.ShippingAddressId,
                createQrCodePaymentDto.CustomerCouponId);

        var orderId = BusinessRuleConstants.Order.GenerateUniqueOrderId();
        var order = new Order
        {
            Id = orderId,
            OrderDate = DateTime.UtcNow,
            OrderStatus = OrderStatus.PendingPayment,
            PaymentMethod = PaymentMethod.QRCode,
            TotalAmountBeforeDiscount = totalAmountBeforeDiscount,
            TotalAmount = totalAmount,
            LoyaltyPointsEarned = loyaltyPointsEarned,
            ShippingAddressSnapshot = new ShippingAddressSnapshot
            {
                RecipientName = shippingAddress.RecipientName,
                RecipientPhoneNumber = shippingAddress.RecipientPhoneNumber,
                SpecificAddress = shippingAddress.SpecificAddress,
                CommuneName = shippingAddress.Commune!.Name,
                ProvinceName = shippingAddress.Commune.Province!.Name
            },
            CustomerCouponId = createQrCodePaymentDto.CustomerCouponId,
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

    private async
        Task<(ShippingAddress shippingAddress, long totalAmountBeforeDiscount, long totalAmount, int loyaltyPointsEarned
            , List<OrderItem>orderItems)> PrepareOrderAsync(int customerId, int shippingAddressId,
            int? customerCouponId)
    {
        context.ChangeTracker.Clear();

        var shippingAddress = await context.ShippingAddresses.AsNoTracking()
            .Include(sa => sa.Commune)
            .ThenInclude(c => c!.Province)
            .FirstOrDefaultAsync(sa => sa.Id == shippingAddressId && sa.CustomerId == customerId);

        if (shippingAddress == null)
        {
            throw new Exception("Địa chỉ giao hàng không tồn tại hoặc không thuộc về khách hàng.");
        }

        Coupon? coupon = null;
        if (customerCouponId.HasValue)
        {
            var customerCoupon = await context.CustomerCoupons.FromSqlInterpolated(
                    $"""
                     SELECT *
                     FROM "CustomerCoupons"
                     WHERE "Id" = {customerCouponId} AND "CustomerId" = {customerId} AND "IsUsed" = false AND "ExpiryDate" > now()
                     FOR NO KEY UPDATE OF "CustomerCoupons"
                     """)
                .Include(cc => cc.Coupon)
                .FirstOrDefaultAsync();

            if (customerCoupon == null)
            {
                throw new Exception(
                    "Mã giảm giá không hợp lệ, đã được sử dụng, hết hạn hoặc không thuộc về khách hàng.");
            }

            coupon = customerCoupon.Coupon;
            customerCoupon.IsUsed = true;
        }

        var selectedCartItems = await context.CartItems.FromSqlInterpolated(
                $"""
                 SELECT * 
                 FROM "CartItems"
                 WHERE "CustomerId" = {customerId} AND "IsSelected" = true
                 ORDER BY "ProductId"
                 FOR NO KEY UPDATE
                 """)
            .ToListAsync();

        if (selectedCartItems.Count == 0)
        {
            throw new Exception("Không có sản phẩm nào được chọn trong giỏ hàng.");
        }

        var products = await context.Products.FromSqlInterpolated(
                $"""
                 SELECT *
                 FROM "Products"
                 WHERE "Id" = ANY({selectedCartItems.Select(ci => ci.ProductId)})
                 ORDER BY "Id"
                 FOR NO KEY UPDATE OF "Products"
                 """)
            .Include(p => p.ProductUnit)
            .ToListAsync();

        var orderItems = new List<OrderItem>();
        foreach (var cartItem in selectedCartItems)
        {
            var product = products.FirstOrDefault(p => p.Id == cartItem.ProductId);
            if (product is not { IsActive: true })
            {
                throw new Exception($"Một số sản phẩm trong đơn hàng không tồn tại hoặc đã ngừng kinh doanh.");
            }

            if (product.Quantity < cartItem.Quantity)
            {
                throw new Exception($"Một số sản phẩm trong đơn hàng không đủ số lượng trong kho");
            }

            HoldProducts(product, cartItem.Quantity);

            orderItems.Add(new OrderItem
            {
                ProductId = cartItem.ProductId,
                ProductSnapshot = new ProductSnapshot
                {
                    Name = product.Name,
                    ImageFilePath = product.ImageFilePath,
                    UnitPrice = product.Price,
                    ProductUnitName = product.ProductUnit!.Name
                },
                Quantity = cartItem.Quantity
            });
        }

        context.CartItems.RemoveRange(selectedCartItems);

        var totalAmountBeforeDiscount = orderItems.Sum(oi => oi.Quantity * oi.ProductSnapshot.UnitPrice);
        var discountAmount = coupon != null
            ? coupon.DiscountType switch
            {
                DiscountType.Percentage => Math.Min((long)(totalAmountBeforeDiscount * (coupon.DiscountValue / 100m)),
                    coupon.MaxDiscountAmount ?? long.MaxValue),
                DiscountType.FixedAmount => coupon.DiscountValue,
                _ => throw new Exception("Loại giảm giá không hợp lệ.")
            }
            : 0;
        var totalAmount = Math.Max(0, totalAmountBeforeDiscount - discountAmount);
        return (shippingAddress, totalAmountBeforeDiscount, totalAmount,
            BusinessRuleConstants.LoyaltyPoint.CalculateLoyaltyPoints(totalAmount), orderItems);
    }

    public async Task CancelCashOnDeliveryOrderAsync(int customerId, long orderId)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)!
            .ThenInclude(oi => oi.Product)
            .Include(o => o.CustomerCoupon)
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

        order.CustomerCoupon?.IsUsed = false;
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
            .Include(o => o.CustomerCoupon)
            .Include(order => order.Customer)
            .ThenInclude(c => c!.Customer)
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
        _ = emailService.SendEmailAsync(order.Customer!.Customer!.Email!, "Xác nhận thanh toán thành công",
            $"Thanh toán cho đơn hàng {order.Id} đã được xác nhận thành công. Đơn hàng của bạn đang được xử lý và sẽ sớm được giao đến bạn. Cảm ơn bạn đã mua sắm tại FruitShop!");
    }

    public async Task CancelQrCodePaymentOrderByPayOsCallbackAsync(long orderId)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)!
            .ThenInclude(oi => oi.Product)
            .Include(o => o.CustomerCoupon)
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

        order.CustomerCoupon?.IsUsed = false;
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
            .Include(o => o.CustomerCoupon)
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

        order.CustomerCoupon?.IsUsed = false;
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
            .Include(o => o.CustomerCoupon)
            .Where(o => o.PaymentMethod == PaymentMethod.QRCode
                        && o.OrderStatus == OrderStatus.PendingPayment
                        && o.QrCodePaymentData!.ExpirationDate < DateTime.UtcNow)
            .ToListAsync();

        foreach (var order in expiredOrders)
        {
            order.CustomerCoupon?.IsUsed = false;
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
            query = query.WhereContainsUnaccent(searchId, o => o.Id);
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
            .ApplyPaging(pagedAndSortedRequest.PageIndex, pagedAndSortedRequest.PageSize)
            .ToListAsync();
        return new PagedAndSortedDto<OrderSummaryDto>(mapper.ToOrderSummaryDtoList(orders), totalCount,
            pagedAndSortedRequest.PageIndex, pagedAndSortedRequest.PageSize, pagedAndSortedRequest.SortColumn,
            pagedAndSortedRequest.SortDirection.Value);
    }

    public async Task<OrderDetailDto> GetOrderDetailAsync(int customerId, long orderId)
    {
        var order = await context.Orders.AsNoTracking()
            .Include(o => o.Shipper)
            .ThenInclude(s => s!.Shipper)
            .Include(o => o.OrderItems)!
            .ThenInclude(oi => oi.ProductReview)
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