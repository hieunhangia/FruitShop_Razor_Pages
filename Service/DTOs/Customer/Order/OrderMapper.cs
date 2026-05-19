using Repository.Models.Orders;
using Repository.Models.Users;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Order;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OrderMapper(FileService fileService)
{
    public partial List<OrderSummaryDto> ToOrderSummaryDtoList(List<Repository.Models.Orders.Order> orders);

    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.ProductUnitName)}",
        nameof(OrderItemDto.ProductUnitName))]
    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.UnitPrice)}",
        nameof(OrderItemDto.UnitPrice))]
    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.Name)}",
        nameof(OrderItemDto.ProductName))]
    [MapProperty($"{nameof(OrderItem.ProductSnapshot)}.{nameof(ProductSnapshot.ImageFilePath)}",
        nameof(OrderItemDto.ProductImageFileUrl), Use = nameof(MapProductImageFilePath))]
    public partial OrderItemDto ToOrderItemDto(OrderItem orderItem);

    [UserMapping(Default = false)]
    private string MapProductImageFilePath(string imageFilePath) => fileService.GetPublicFileUrl(imageFilePath);

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
    public partial OrderDetailDto ToOrderDetailDto(Repository.Models.Orders.Order order);
}