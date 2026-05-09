namespace Service.DTOs.Customer.Cart;

public class CartDto
{
    public required List<CartItemDto> CartItems { get; set; }
    public required bool HasUpdates { get; set; }
}