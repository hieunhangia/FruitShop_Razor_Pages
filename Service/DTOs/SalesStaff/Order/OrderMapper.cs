using Repository.Constants;
using Repository.Models.Orders;
using Repository.Models.Users;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.SalesStaff.Order;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class OrderMapper
{

    private static string MapOrderStatus(OrderStatus status)
    {
        switch (status)
        {
            case OrderStatus.Cancelled:
                return "Đã huỷ";
            case OrderStatus.Delivered:
                return "Đã giao";
            case OrderStatus.PendingConfirmation:
                return "Chờ xác nhận";
            case OrderStatus.PendingPayment:
                return "Chờ thanh toán";
            case OrderStatus.Processing:
                return "Đang xử lý";
            case OrderStatus.Shipping:
                return "Đang giao";
            default:
                return status.ToString();
        }
    }

    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.ProductUnitName)}",
        nameof(OrderItemDto.ProductUnitName))]
    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.UnitPrice)}",
        nameof(OrderItemDto.UnitPrice))]
    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.Name)}",
        nameof(OrderItemDto.ProductName))]
    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.ImageFilePath)}",
        nameof(OrderItemDto.ProductImageFilePath))]
    private static partial OrderItemDto ToOrderItemDto(OrderItem orderItem);

    [MapProperty(nameof(Repository.Models.Orders.Order.ShippingAddressSnapshot),
        nameof(OrderListDto.ShippingAddress))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.Customer)}.{nameof(CustomerData.Customer)}.{nameof(User.Email)}",
        nameof(OrderListDto.CustomerEmail))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.Shipper)}.{nameof(ShipperData.ShipperName)}",
        nameof(OrderListDto.ShipperName))]
    private static partial OrderListDto ToOrderListDto(Repository.Models.Orders.Order order);
        
    [MapProperty(nameof(Repository.Models.Orders.Order.ShippingAddressSnapshot),
        nameof(OrderDetailDto.ShippingAddress))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.Customer)}.{nameof(CustomerData.Customer)}.{nameof(User.Email)}",
        nameof(OrderDetailDto.CustomerEmail))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.Shipper)}.{nameof(ShipperData.ShipperName)}",
        nameof(OrderDetailDto.ShipperName))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.Shipper)}.{nameof(ShipperData.Shipper)}.{nameof(User.PhoneNumber)}",
        nameof(OrderDetailDto.ShipperPhoneNumber))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.QrCodePaymentData)}.{nameof(OrderQrCodePaymentData.ExpirationDate)}",
        nameof(OrderDetailDto.QrCodePaymentExpiration))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.QrCodePaymentData)}.{nameof(OrderQrCodePaymentData.PaymentDate)}",
        nameof(OrderDetailDto.QrCodePaymentDate))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.OrderStatus)}",
        nameof(OrderDetailDto.OrderStatusName))]
    private static partial OrderDetailDto ToOrderDetailDto(Repository.Models.Orders.Order order);

    public static partial IQueryable<OrderDetailDto> ProjectToOrderDetailDto(
        this IQueryable<Repository.Models.Orders.Order> order);

    public static partial IQueryable<OrderListDto> ProjectToOrderListDto(
        this IQueryable<Repository.Models.Orders.Order> order);
}