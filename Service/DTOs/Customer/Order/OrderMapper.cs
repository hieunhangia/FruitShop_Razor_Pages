using Repository.Models.Orders;
using Repository.Models.Users;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Order;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OrderMapper
{
    public partial List<OrderSummaryDto> ToOrderSummaryDtoList(List<Repository.Models.Orders.Order> orders);

    [MapProperty(nameof(OrderItem.ProductSnapshot), nameof(OrderItemDto.Product))]
    public partial OrderItemDto ToOrderItemDto(OrderItem orderItem);

    [MapProperty(nameof(Repository.Models.Orders.Order.ShippingAddressSnapshot),
        nameof(OrderDetailDto.ShippingAddress))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.Shipper)}.{nameof(User.ShipperInformation)}.{nameof(ShipperInformation.ShipperName)}",
        nameof(OrderDetailDto.ShipperName))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.QrCodePaymentData)}.{nameof(OrderQrCodePaymentData.PaymentLink)}",
        nameof(OrderDetailDto.QrCodePaymentLink))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.QrCodePaymentData)}.{nameof(OrderQrCodePaymentData.ExpirationDate)}",
        nameof(OrderDetailDto.QrCodePaymentExpiration))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.QrCodePaymentData)}.{nameof(OrderQrCodePaymentData.PaymentDate)}",
        nameof(OrderDetailDto.QrCodePaymentDate))]
    public partial OrderDetailDto ToOrderDetailDto(Repository.Models.Orders.Order order);
}