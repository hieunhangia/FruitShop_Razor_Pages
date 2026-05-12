namespace Service.DTOs.Customer.Cart;

public class CartItemDto
{
    public required int ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string ProductImageUrl { get; set; }
    public required string ProductUnitName { get; set; }
    public required long ProductPrice { get; set; }
    public required int Quantity { get; set; }
    public required bool IsSelected { get; set; }
}