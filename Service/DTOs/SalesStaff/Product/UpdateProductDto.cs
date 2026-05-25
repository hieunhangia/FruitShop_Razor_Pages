namespace Service.DTOs.SalesStaff;

public class UpdateProductDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public long Price { get; set; }
    public int Quantity { get; set; }
    public int ProductUnitId { get; set; }
    public List<int> CategoryIds { get; set; } = [];
}