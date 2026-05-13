namespace Service.DTOs.Guest.Homepage;
public class ProductDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public required long Price { get; set; }
    public required int Quantity { get; set; }
    public required int HeldQuantity { get; set; }
    public required string ProductUnitName { get; set; }

    public int AvailableQuantity => Quantity - HeldQuantity;
}