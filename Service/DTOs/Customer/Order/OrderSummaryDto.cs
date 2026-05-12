using Repository.Constants;

namespace Service.DTOs.Customer.Order;

public class OrderSummaryDto
{
    public required long Id { get; set; }
    public required DateTime OrderDate { get; set; }
    public required OrderStatus OrderStatus { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public required long TotalAmount { get; set; }
}