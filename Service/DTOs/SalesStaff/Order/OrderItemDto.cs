namespace Service.DTOs.SalesStaff.Order;
public class OrderItemDto
{
	public required int ProductId { get; set; }
	public required string ProductName { get; set; }
	public required string ProductUnitName { get; set; }
	public required long UnitPrice { get; set; }
	public required int Quantity { get; set; }
	public required string ProductImageFilePath { get; set; }
}