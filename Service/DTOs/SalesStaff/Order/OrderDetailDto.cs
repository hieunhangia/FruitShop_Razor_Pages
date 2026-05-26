using Repository.Constants;
using Repository.Models.Orders;

namespace Service.DTOs.SalesStaff.Order;

public class OrderDetailDto
{
	public required long Id { get; set; }
	public required DateTime OrderDate { get; set; }
	public required OrderStatus OrderStatus { get; set; }
	public required PaymentMethod PaymentMethod { get; set; }
	public required long TotalAmountBeforeDiscount { get; set; }
	public required long TotalAmount { get; set; }
	public required ShippingAddressSnapshot ShippingAddress { get; set; }
	public required int CustomerId { get; set; }
	public required string CustomerEmail { get; set; }
	public required List<OrderItemSummaryDto> OrderItems { get; set; }
	public string? ShipperName { get; set; }
	public string? ShipperPhoneNumber { get; set; }
	public DateTime? QrCodePaymentExpiration { get; set; }
	public DateTime? QrCodePaymentDate { get; set; }
}

public class OrderItemSummaryDto
{
	public required int ProductId { get; set; }
	public required string ProductName { get; set; }
	public required string ProductUnitName { get; set; }
	public required long UnitPrice { get; set; }
	public required int Quantity { get; set; }
	public required string ProductImageFilePath { get; set; }
}