namespace Service.DTOs.SalesStaff.Order;

public class OrderFilter
{
	public Repository.Constants.OrderStatus? OrderStatus { get; set; }
	public Repository.Constants.PaymentMethod? PaymentMethod { get; set; }
}