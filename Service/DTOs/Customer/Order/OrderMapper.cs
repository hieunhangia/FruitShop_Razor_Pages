using Repository.Models.Orders;
using Repository.Models.Users;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Order;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OrderMapper
{
    public partial List<OrderSummaryDto> ToOrderSummaryDtoList(List<Repository.Models.Orders.Order> orders);

    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.ProductUnitName)}",
        nameof(OrderItemDto.ProductUnitName))]
    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.UnitPrice)}",
        nameof(OrderItemDto.UnitPrice))]
    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.Name)}",
        nameof(OrderItemDto.ProductName))]
    [MapperIgnoreTarget(nameof(OrderItemDto.ProductImageFileUrl))]
    private partial OrderItemDto ToOrderItemDtoBasic(OrderItem orderItem);

    [MapProperty(nameof(Repository.Models.Orders.Order.ShippingAddressSnapshot),
        nameof(OrderDetailDto.ShippingAddress))]
    [MapProperty(
        $"{nameof(Repository.Models.Orders.Order.Shipper)}.{nameof(User.ShipperData)}.{nameof(ShipperData.ShipperName)}",
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
    private partial OrderDetailDto ToOrderDetailDtoBasic(Repository.Models.Orders.Order order);

    public OrderDetailDto ToOrderDetailDto(Repository.Models.Orders.Order order,
        Func<string, string> getProductImageFileUrl)
    {
        var orderDetailDto = ToOrderDetailDtoBasic(order);
        var orderItemsList = order.OrderItems!.ToList();
        for (var i = 0; i < orderItemsList.Count; i++)
        {
            orderDetailDto.OrderItems[i].ProductImageFileUrl =
                getProductImageFileUrl(orderItemsList[i].ProductSnapshot.ImageFilePath);
        }

        return orderDetailDto;
    }
}