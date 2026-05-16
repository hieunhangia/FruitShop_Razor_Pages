using Repository.Constants;

namespace Service.DTOs.Customer.Order;

public class OrderShippingDto
{
    public required ShippingStatus ShippingStatus { get; set; }
    public required DateTime OccurredAt { get; set; }
}