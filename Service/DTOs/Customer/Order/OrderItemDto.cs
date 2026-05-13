using Repository.Models.Orders;

namespace Service.DTOs.Customer.Order;

public class OrderItemDto
{
    public required ProductSnapshot Product { get; set; }
    public required int Quantity { get; set; }
}