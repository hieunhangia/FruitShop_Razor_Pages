using Repository.Models.Orders;
using Repository.Models.Users;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Order;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class OrderMapper
{
    public static partial IQueryable<OrderSummaryDto> ProjectToOrderSummaryDto(
        this IQueryable<Repository.Models.Orders.Order> order);

    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.ProductUnitName)}",
        nameof(OrderItemDto.ProductUnitName))]
    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.UnitPrice)}",
        nameof(OrderItemDto.UnitPrice))]
    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.Name)}",
        nameof(OrderItemDto.ProductName))]
    [MapperIgnoreTarget(nameof(OrderItemDto.ProductImageFileUrl))]
    [MapperIgnoreTarget(nameof(OrderItemDto.HasReview))]
    private static partial OrderItemDto ToOrderItemDto(OrderItem orderItem);

    [MapProperty(nameof(Repository.Models.Orders.Order.ShippingAddressSnapshot),
        nameof(OrderDetailDto.ShippingAddress))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.Shipper)}.{nameof(ShipperData.ShipperName)}",
        nameof(OrderDetailDto.ShipperName))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.Shipper)}.{nameof(ShipperData.Shipper)}.{nameof(User.PhoneNumber)}",
        nameof(OrderDetailDto.ShipperPhoneNumber))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.QrCodePaymentData)}.{nameof(OrderQrCodePaymentData.PaymentLink)}",
        nameof(OrderDetailDto.QrCodePaymentLink))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.QrCodePaymentData)}.{nameof(OrderQrCodePaymentData.ExpirationDate)}",
        nameof(OrderDetailDto.QrCodePaymentExpiration))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.QrCodePaymentData)}.{nameof(OrderQrCodePaymentData.PaymentDate)}",
        nameof(OrderDetailDto.QrCodePaymentDate))]
    private static partial OrderDetailDto ToOrderDetailDto(Repository.Models.Orders.Order order);

    public static partial IQueryable<OrderDetailDto> ProjectToOrderDetailDto(
        this IQueryable<Repository.Models.Orders.Order> order);
}