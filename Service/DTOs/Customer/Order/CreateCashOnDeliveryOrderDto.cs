namespace Service.DTOs.Customer.Order;

public class CreateCashOnDeliveryOrderDto
{
    public required string CustomerEmail { get; set; }
    public required int ShippingAddressId { get; set; }
}