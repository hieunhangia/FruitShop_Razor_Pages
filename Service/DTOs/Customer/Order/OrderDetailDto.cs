using Repository.Constants;
using Repository.Models.Orders;

namespace Service.DTOs.Customer.Order;

public class OrderDetailDto
{
    public required long Id { get; set; }
    public required DateTime OrderDate { get; set; }
    public required OrderStatus OrderStatus { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public required ShippingAddressSnapshot ShippingAddress { get; set; }
    public required long TotalAmountBeforeDiscount { get; set; }
    public required long TotalAmount { get; set; }
    public required long LoyaltyPointsEarned { get; set; }
    public required List<OrderItemDto> OrderItems { get; set; }
    public string? ShipperName { get; set; }
    public string? ShipperPhoneNumber { get; set; }
    public string? QrCodePaymentLink { get; set; }
    public DateTime? QrCodePaymentExpiration { get; set; }
    public DateTime? QrCodePaymentDate { get; set; }
    public List<OrderShippingDto>? OrderShippings { get; set; }
}