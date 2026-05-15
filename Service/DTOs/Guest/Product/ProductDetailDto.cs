namespace Service.DTOs.Guest.DetailProduct;

public class ProductDetailDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public required long Price { get; set; }
    public required int Quantity { get; set; }
    public required int HeldQuantity { get; set; }
    public required bool IsActive { get; set; }
    public required string ProductUnitName { get; set; }
    public List<CategoryDto> Categories { get; set; } = new();

    public int AvailableQuantity => Quantity - HeldQuantity;
}

public class CategoryDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
}