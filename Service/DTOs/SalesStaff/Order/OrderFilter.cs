namespace Service.DTOs.SalesStaff.Order;

public class OrderFilter
{
	public Repository.Constants.OrderStatus? OrderStatus { get; set; }
	public Repository.Constants.PaymentMethod? PaymentMethod { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
}