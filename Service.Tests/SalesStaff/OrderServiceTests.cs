using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Repository;
using Repository.Constants;
using Repository.Models.Coupons;
using Repository.Models.Orders;
using Repository.Models.Products;
using Repository.Models.Users;
using Service.SalesStaff;

namespace Service.Tests.SalesStaff;

/// <summary>
/// Fake EmailService – bỏ qua mọi gọi SMTP thật, chỉ ghi lại các email đã "gửi".
/// </summary>
public class FakeEmailService : EmailService
{
    public List<(string ToEmail, string Subject, string Body)> SentEmails { get; } = [];

    public FakeEmailService()
        : base(
            NullLogger<EmailService>.Instance,
            BuildFakeConfiguration())
    {
    }

    public new Task SendEmailAsync(string toEmail, string subject, string message)
    {
        SentEmails.Add((toEmail, subject, message));
        return Task.CompletedTask;
    }

    private static IConfiguration BuildFakeConfiguration()
    {
        var values = new Dictionary<string, string?>
        {
            ["EmailSender:SmtpHost"] = "localhost",
            ["EmailSender:SmtpPort"] = "25",
            ["EmailSender:Username"] = "test",
            ["EmailSender:Password"] = "test",
            ["EmailSender:FromName"] = "FruitShop",
            ["EmailSender:FromEmail"] = "noreply@fruitshop.test",
        };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}

/// <summary>
/// Fake FileService – trả về URL giả, không kết nối Minio thật.
/// </summary>
public class FakeFileService : FileService
{
    public FakeFileService()
        : base(
            Substitute.For<Minio.IMinioClient>(),
            BuildFakeConfiguration(),
            NullLogger<FileService>.Instance)
    {
    }

    public new string GetPublicFileUrl(string filePath) => $"https://cdn.fake/{filePath}";

    private static IConfiguration BuildFakeConfiguration()
    {
        var values = new Dictionary<string, string?>
        {
            ["MinioSettings:PublicBucketName"] = "public",
            ["MinioSettings:PrivateBucketName"] = "private",
            ["MinioSettings:Endpoint"] = "localhost:9000",
            ["MinioSettings:UseSSL"] = "false",
        };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}

/// <summary>
/// Tạo AppDbContext in-memory dùng chung cho tất cả test.
/// </summary>
public static class TestDbContextFactory
{
    public static AppDbContext Create(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .ConfigureWarnings(w =>
                w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        return new AppDbContext(options);
    }
}

// ============================================================
// Test cases
// ============================================================

public class OrderServiceConfirmCodOrderTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly OrderService _sut;

    public OrderServiceConfirmCodOrderTests()
    {
        _context = TestDbContextFactory.Create($"ConfirmCod_{Guid.NewGuid()}");
        _sut = new OrderService(_context, new FakeFileService(),new FakeEmailService());
    }

    public void Dispose() => _context.Dispose();

    // ─── helpers ────────────────────────────────────────────────────────────────

    private async Task SeedValidCodOrderAsync(
        long orderId = 1,
        int productQuantity = 10,
        int heldQuantity = 3,
        int orderItemQty = 3,
        long loyaltyPoints = 50,
        bool withCustomerEmail = true)
    {
        var product = new Product
        {
            Id = (int)orderId * 100,
            Name = $"Táo Fuji #{orderId}",
            Description = "Táo ngon",
            Price = 50_000,
            Quantity = productQuantity,
            HeldQuantity = heldQuantity,
            ImageFilePath = "fruits/apple.jpg",
            DisplayOrder = (int)orderId,
            ProductUnitId = 1,
        };

        User? userEntity = null;
        CustomerData? customerData = null;

        if (withCustomerEmail)
        {
            userEntity = new User
            {
                Id = (int)orderId * 10,
                UserName = $"customer_{orderId}@test.com",
                Email = $"customer_{orderId}@test.com",
            };
            customerData = new CustomerData
            {
                CustomerId = userEntity.Id,
                Customer = userEntity,
                LoyaltyPoints = 100,
            };
        }

        var order = new Order
        {
            Id = orderId,
            OrderDate = DateTime.UtcNow,
            OrderStatus = OrderStatus.PendingConfirmation,
            PaymentMethod = PaymentMethod.CashOnDelivery,
            TotalAmountBeforeDiscount = 150_000,
            TotalAmount = 150_000,
            LoyaltyPointsEarned = loyaltyPoints,
            CustomerId = customerData?.CustomerId ?? 999,
            Customer = customerData,
            ShippingAddressSnapshot = new ShippingAddressSnapshot
            {
                RecipientName = "Nguyễn Văn A",
                RecipientPhoneNumber = "0901234567",
                SpecificAddress = "123 Đường ABC",
                CommuneName = "Phường 1",
                ProvinceName = "TP. HCM",
            },
            OrderItems = new List<OrderItem>
            {
                new()
                {
                    OrderId = orderId,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = orderItemQty,
                    ProductSnapshot = new ProductSnapshot
                    {
                        Name = product.Name,
                        ImageFilePath = product.ImageFilePath,
                        ProductUnitName = "kg",
                        UnitPrice = product.Price,
                    },
                },
            },
        };

        _context.Products.Add(product);
        if (userEntity != null) _context.Users.Add(userEntity);
        if (customerData != null) _context.CustomerData.Add(customerData);
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    // ─── happy-path ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Khi xác nhận đơn COD hợp lệ, trạng thái phải chuyển sang Processing.
    /// </summary>
    [Fact]
    public async Task ConfirmCodOrder_ValidOrder_ChangesStatusToProcessing()
    {
        await SeedValidCodOrderAsync(orderId: 1);

        await _sut.ConfirmCodOrderAsync(1);

        var order = await _context.Orders.FindAsync(1L);
        Assert.Equal(OrderStatus.Processing, order!.OrderStatus);
    }

    /// <summary>
    /// Khi xác nhận, HeldQuantity của sản phẩm phải giảm đúng số lượng trong đơn.
    /// </summary>
    [Fact]
    public async Task ConfirmCodOrder_ValidOrder_DecreasesProductHeldQuantity()
    {
        const int initialHeld = 5;
        const int orderQty = 3;
        await SeedValidCodOrderAsync(orderId: 2, heldQuantity: initialHeld, orderItemQty: orderQty);

        await _sut.ConfirmCodOrderAsync(2);

        var product = await _context.Products.FirstAsync(p => p.Id == 200);
        Assert.Equal(initialHeld - orderQty, product.HeldQuantity);
    }

    /// <summary>
    /// Khi xác nhận, điểm tích luỹ phải được cộng vào CustomerData.
    /// </summary>
    [Fact]
    public async Task ConfirmCodOrder_ValidOrder_AddsLoyaltyPointsToCustomer()
    {
        const long initialPoints = 100;
        const long earnedPoints = 50;
        await SeedValidCodOrderAsync(orderId: 3, loyaltyPoints: earnedPoints);

        await _sut.ConfirmCodOrderAsync(3);

        var customer = await _context.CustomerData.FindAsync(30);
        Assert.Equal(initialPoints + earnedPoints, customer!.LoyaltyPoints);
    }

    // ─── error cases ────────────────────────────────────────────────────────────

    /// <summary>
    /// Đơn không tồn tại → ném Exception với thông báo phù hợp.
    /// </summary>
    [Fact]
    public async Task ConfirmCodOrder_OrderNotFound_ThrowsException()
    {
        var ex = await Assert.ThrowsAsync<Exception>(() => _sut.ConfirmCodOrderAsync(9999));
        Assert.Contains("không tồn tại", ex.Message);
    }

    /// <summary>
    /// Đơn đang ở trạng thái Processing (không phải PendingConfirmation) → ném Exception.
    /// </summary>
    [Fact]
    public async Task ConfirmCodOrder_OrderAlreadyProcessing_ThrowsException()
    {
        await SeedValidCodOrderAsync(orderId: 4);
        var order = await _context.Orders.FindAsync(4L);
        order!.OrderStatus = OrderStatus.Processing;
        await _context.SaveChangesAsync();

        var ex = await Assert.ThrowsAsync<Exception>(() => _sut.ConfirmCodOrderAsync(4));
        Assert.Contains("chờ xác nhận", ex.Message);
    }

    /// <summary>
    /// Đơn thanh toán bằng QR Code (không phải COD) → ném Exception.
    /// </summary>
    [Fact]
    public async Task ConfirmCodOrder_QrCodePayment_ThrowsException()
    {
        await SeedValidCodOrderAsync(orderId: 5);
        var order = await _context.Orders.FindAsync(5L);
        order!.PaymentMethod = PaymentMethod.QRCode;
        await _context.SaveChangesAsync();

        var ex = await Assert.ThrowsAsync<Exception>(() => _sut.ConfirmCodOrderAsync(5));
        Assert.Contains("thanh toán khi nhận hàng", ex.Message);
    }

    /// <summary>
    /// Đơn đã bị huỷ → ném Exception.
    /// </summary>
    [Fact]
    public async Task ConfirmCodOrder_CancelledOrder_ThrowsException()
    {
        await SeedValidCodOrderAsync(orderId: 6);
        var order = await _context.Orders.FindAsync(6L);
        order!.OrderStatus = OrderStatus.Cancelled;
        await _context.SaveChangesAsync();

        var ex = await Assert.ThrowsAsync<Exception>(() => _sut.ConfirmCodOrderAsync(6));
        Assert.Contains("chờ xác nhận", ex.Message);
    }
}

// ============================================================

public class OrderServiceCancelCodOrderTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly OrderService _service;

    public OrderServiceCancelCodOrderTests()
    {
        _context = TestDbContextFactory.Create($"CancelCod_{Guid.NewGuid()}");
        _service = new OrderService(_context, new FakeFileService(), new FakeEmailService());
    }

    public void Dispose() => _context.Dispose();

    // ─── helpers ────────────────────────────────────────────────────────────────

    private async Task SeedValidCodOrderAsync(
        long orderId = 1,
        int productQuantity = 10,
        int heldQuantity = 3,
        int orderItemQty = 3,
        bool withCoupon = false,
        bool withCustomerEmail = true)
    {
        var product = new Product
        {
            Id = (int)orderId * 100,
            Name = $"Xoài cát #{orderId}",
            Description = "Xoài ngon",
            Price = 30_000,
            Quantity = productQuantity,
            HeldQuantity = heldQuantity,
            ImageFilePath = "fruits/mango.jpg",
            DisplayOrder = (int)orderId + 100,
            ProductUnitId = 1,
        };

        User? userEntity = null;
        CustomerData? customerData = null;

        if (withCustomerEmail)
        {
            userEntity = new User
            {
                Id = (int)orderId * 10,
                UserName = $"cancel_customer_{orderId}@test.com",
                Email = $"cancel_customer_{orderId}@test.com",
            };
            customerData = new CustomerData
            {
                CustomerId = userEntity.Id,
                Customer = userEntity,
                LoyaltyPoints = 200,
            };
        }

        CustomerCoupon? coupon = null;
        if (withCoupon)
        {
            coupon = new CustomerCoupon
            {
                Id = (int)orderId * 1000,
                IsUsed = true,
                CustomerId = customerData?.CustomerId ?? 999,
            };
            _context.CustomerCoupons.Add(coupon);
        }

        var order = new Order
        {
            Id = orderId,
            OrderDate = DateTime.UtcNow,
            OrderStatus = OrderStatus.PendingConfirmation,
            PaymentMethod = PaymentMethod.CashOnDelivery,
            TotalAmountBeforeDiscount = 90_000,
            TotalAmount = 90_000,
            LoyaltyPointsEarned = 10,
            CustomerId = customerData?.CustomerId ?? 999,
            Customer = customerData,
            CustomerCoupon = coupon,
            ShippingAddressSnapshot = new ShippingAddressSnapshot
            {
                RecipientName = "Trần Thị B",
                RecipientPhoneNumber = "0909876543",
                SpecificAddress = "456 Đường XYZ",
                CommuneName = "Phường 5",
                ProvinceName = "Hà Nội",
            },
            OrderItems = new List<OrderItem>
            {
                new()
                {
                    OrderId = orderId,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = orderItemQty,
                    ProductSnapshot = new ProductSnapshot
                    {
                        Name = product.Name,
                        ImageFilePath = product.ImageFilePath,
                        ProductUnitName = "kg",
                        UnitPrice = product.Price,
                    },
                },
            },
        };

        _context.Products.Add(product);
        if (userEntity != null) _context.Users.Add(userEntity);
        if (customerData != null) _context.CustomerData.Add(customerData);
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    // ─── happy-path ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Khi huỷ đơn COD hợp lệ, trạng thái phải chuyển sang Cancelled.
    /// </summary>
    [Fact]
    public async Task CancelCodOrder_ValidOrder_ChangesStatusToCancelled()
    {
        await SeedValidCodOrderAsync(orderId: 1);

        await _service.CancelCodOrderAsync(1);

        var order = await _context.Orders.FindAsync(1L);
        Assert.Equal(OrderStatus.Cancelled, order!.OrderStatus);
    }

    /// <summary>
    /// Khi huỷ, Quantity của sản phẩm phải được hoàn trả đúng số lượng trong đơn.
    /// </summary>
    [Fact]
    public async Task CancelCodOrder_ValidOrder_RestoresProductQuantity()
    {
        const int initialQty = 10;
        const int heldQty = 3;
        const int orderItemQty = 3;
        await SeedValidCodOrderAsync(orderId: 2, productQuantity: initialQty, heldQuantity: heldQty,
            orderItemQty: orderItemQty);

        await _service.CancelCodOrderAsync(2);

        var product = await _context.Products.FirstAsync(p => p.Id == 200);
        Assert.Equal(initialQty + orderItemQty, product.Quantity);
    }

    /// <summary>
    /// Khi huỷ, HeldQuantity của sản phẩm phải giảm đúng số lượng trong đơn.
    /// </summary>
    [Fact]
    public async Task CancelCodOrder_ValidOrder_DecreasesProductHeldQuantity()
    {
        const int heldQty = 5;
        const int orderItemQty = 5;
        await SeedValidCodOrderAsync(orderId: 3, heldQuantity: heldQty, orderItemQty: orderItemQty);

        await _service.CancelCodOrderAsync(3);

        var product = await _context.Products.FirstAsync(p => p.Id == 300);
        Assert.Equal(0, product.HeldQuantity);
    }

    /// <summary>
    /// Khi huỷ đơn có coupon, IsUsed của coupon phải được đặt lại thành false.
    /// </summary>
    [Fact]
    public async Task CancelCodOrder_OrderWithCoupon_SetsCouponIsUsedToFalse()
    {
        await SeedValidCodOrderAsync(orderId: 4, withCoupon: true);

        await _service.CancelCodOrderAsync(4);

        var coupon = await _context.CustomerCoupons.FindAsync(4000);
        Assert.False(coupon!.IsUsed);
    }

    /// <summary>
    /// Khi huỷ đơn không có coupon, không có exception nào được ném.
    /// </summary>
    [Fact]
    public async Task CancelCodOrder_OrderWithoutCoupon_CompletesSuccessfully()
    {
        await SeedValidCodOrderAsync(orderId: 5, withCoupon: false);

        var ex = await Record.ExceptionAsync(() => _service.CancelCodOrderAsync(5));

        Assert.Null(ex);
    }

    // ─── error cases ────────────────────────────────────────────────────────────

    /// <summary>
    /// Đơn không tồn tại → ném Exception với thông báo phù hợp.
    /// </summary>
    [Fact]
    public async Task CancelCodOrder_OrderNotFound_ThrowsException()
    {
        var ex = await Assert.ThrowsAsync<Exception>(() => _service.CancelCodOrderAsync(9999));
        Assert.Contains("không tồn tại", ex.Message);
    }

    /// <summary>
    /// Đơn đang ở trạng thái Processing → ném Exception.
    /// </summary>
    [Fact]
    public async Task CancelCodOrder_OrderAlreadyProcessing_ThrowsException()
    {
        await SeedValidCodOrderAsync(orderId: 6);
        var order = await _context.Orders.FindAsync(6L);
        order!.OrderStatus = OrderStatus.Processing;
        await _context.SaveChangesAsync();

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.CancelCodOrderAsync(6));
        Assert.Contains("chờ xác nhận", ex.Message);
    }

    /// <summary>
    /// Đơn đã được giao → ném Exception.
    /// </summary>
    [Fact]
    public async Task CancelCodOrder_OrderDelivered_ThrowsException()
    {
        await SeedValidCodOrderAsync(orderId: 7);
        var order = await _context.Orders.FindAsync(7L);
        order!.OrderStatus = OrderStatus.Delivered;
        await _context.SaveChangesAsync();

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.CancelCodOrderAsync(7));
        Assert.Contains("chờ xác nhận", ex.Message);
    }

    /// <summary>
    /// Đơn thanh toán bằng QR Code → ném Exception.
    /// </summary>
    [Fact]
    public async Task CancelCodOrder_QrCodePayment_ThrowsException()
    {
        await SeedValidCodOrderAsync(orderId: 8);
        var order = await _context.Orders.FindAsync(8L);
        order!.PaymentMethod = PaymentMethod.QRCode;
        await _context.SaveChangesAsync();

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.CancelCodOrderAsync(8));
        Assert.Contains("thanh toán khi nhận hàng", ex.Message);
    }
}
