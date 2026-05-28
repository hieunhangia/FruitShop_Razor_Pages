using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Orders;
using Repository.Models.Products;
using Service.DTOs;
using Service.DTOs.SalesStaff.Order;

namespace Service.SalesStaff;

public class OrderService(AppDbContext context, FileService fileService, EmailService emailService)
{
    public async Task<OrderDetailDto> GetOrderDetailByIdAsync(long orderId)
    {

        var orderDto = await context.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId)
            .ProjectToOrderDetailDto()
            .AsSplitQuery()
            .FirstOrDefaultAsync();
        
        if (orderDto == null)
        {
            throw new Exception("Đơn hàng không tồn tại.");
        }

        foreach (var item in orderDto.OrderItems)
        {
            item.ProductImageFilePath = fileService.GetPublicFileUrl(item.ProductImageFilePath);
        }

        return orderDto;
    }

    
    public async Task<PagedAndSortedDto<OrderListDto>> GetOrderListAsync(PagedAndSortedRequest<OrderFilter> request)
    {
        var query = context.Orders.AsNoTracking().AsQueryable();

        request.SortColumn ??= nameof(Order.OrderDate);
        request.SortDirection ??= SortDirection.Descending;

        var totalCount = await query.CountAsync();
        if (totalCount == 0)
        {
            return new PagedAndSortedDto<OrderListDto>([], 0, request.PageIndex, request.PageSize,
                request.SortColumn, request.SortDirection.Value);
        }

        var items = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ProjectToOrderListDto()
            .ToListAsync();

        return new PagedAndSortedDto<OrderListDto>(items, totalCount, request.PageIndex, request.PageSize,
            request.SortColumn, request.SortDirection.Value);
    }

    public async Task ConfirmCodOrderAsync(long orderId)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)!
            .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .ThenInclude(c => c!.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception("Đơn hàng không tồn tại.");
        }

        if (order.OrderStatus != OrderStatus.PendingConfirmation ||
            order.PaymentMethod != PaymentMethod.CashOnDelivery)
        {
            throw new Exception(
                "Đơn hàng không ở trạng thái chờ xác nhận hoặc không phải đơn thanh toán khi nhận hàng.");
        }

        order.OrderStatus = OrderStatus.Processing;
        foreach (var orderItem in order.OrderItems!)
        {
            FinalizeProducts(orderItem.Product!, orderItem.Quantity);
        }

        // Cộng điểm loyalty cho khách hàng
        if (order.Customer?.Customer != null)
        {
            order.Customer.LoyaltyPoints += order.LoyaltyPointsEarned;
        }

        await context.SaveChangesAsync();

        if (order.Customer?.Customer != null)
        {
            _ = emailService.SendEmailAsync(order.Customer.Customer.Email!,
                "Đơn hàng đã được xác nhận",
                $"Đơn hàng #{order.Id} của bạn đã được xác nhận và đang được xử lý. " +
                $"Bạn được cộng {order.LoyaltyPointsEarned} điểm tích luỹ. " +
                $"Cảm ơn bạn đã mua sắm tại FruitShop!");
        }
    }

    public async Task CancelCodOrderAsync(long orderId)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)!
            .ThenInclude(oi => oi.Product)
            .Include(o => o.CustomerCoupon)
            .Include(o => o.Customer)
            .ThenInclude(c => c!.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            throw new Exception("Đơn hàng không tồn tại.");
        }

        if (order.OrderStatus != OrderStatus.PendingConfirmation ||
            order.PaymentMethod != PaymentMethod.CashOnDelivery)
        {
            throw new Exception(
                "Đơn hàng không ở trạng thái chờ xác nhận hoặc không phải đơn thanh toán khi nhận hàng.");
        }

        order.CustomerCoupon?.IsUsed = false;
        order.OrderStatus = OrderStatus.Cancelled;
        foreach (var orderItem in order.OrderItems!)
        {
            ReleaseHeldProducts(orderItem.Product!, orderItem.Quantity);
        }

        await context.SaveChangesAsync();

        if (order.Customer?.Customer != null)
        {
            _ = emailService.SendEmailAsync(order.Customer.Customer.Email!,
                "Đơn hàng đã bị huỷ",
                $"Đơn hàng #{order.Id} của bạn đã bị huỷ bởi cửa hàng. " +
                $"Nếu bạn có thắc mắc, vui lòng liên hệ với chúng tôi. " +
                $"Cảm ơn bạn đã quan tâm đến FruitShop!");
        }
    }

    private static void FinalizeProducts(Product product, int quantity)
    {
        product.HeldQuantity -= quantity;
    }

    private static void ReleaseHeldProducts(Product product, int quantity)
    {
        product.Quantity += quantity;
        product.HeldQuantity -= quantity;
    }
}