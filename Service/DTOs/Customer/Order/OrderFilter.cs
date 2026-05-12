using Repository.Constants;

namespace Service.DTOs.Customer.Order;

public class OrderFilter
{
    public string? SearchId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public OrderStatus? OrderStatus { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public long? StartTotalAmount { get; set; }
    public long? EndTotalAmount { get; set; }
}