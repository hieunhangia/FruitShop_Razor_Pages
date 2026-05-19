namespace Service.DTOs.Customer.Order;

public class OrderItemDto
{
    public required int OrderId { get; set; }
    public required int ProductId { get; set; }
    public required string ProductName { get; set; }
    public string ProductImageFileUrl { get; set; } = string.Empty;
    public required string ProductUnitName { get; set; }
    public required long UnitPrice { get; set; }
    public required int Quantity { get; set; }
    public bool HasReview { get; set; }
}