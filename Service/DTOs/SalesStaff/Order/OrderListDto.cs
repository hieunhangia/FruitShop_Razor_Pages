using Repository.Constants;
using Repository.Models.Orders;

namespace Service.DTOs.SalesStaff.Order;

public class OrderListDto
{
	public required long Id { get; set; }
	public required DateTime OrderDate { get; set; }
	public required string OrderStatus { get; set; }
	public required PaymentMethod PaymentMethod { get; set; }
	public required long TotalAmount { get; set; }
	public required ShippingAddressSnapshot ShippingAddress { get; set; }
	public required string CustomerEmail { get; set; }
	public string? ShipperName { get; set; }
}