namespace Service.DTOs.Customer.Order;

public class CreateOrderDto
{
    public required DateTime OrderDate { get; set; }

    public required int ShippingAddressId { get; set; }
}