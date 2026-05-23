namespace Service.DTOs.Everyone.Product;

public class ProductSummaryDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required long Price { get; set; }
    public required string ImageUrl { get; set; }
    public required string ProductUnitName { get; set; }
    public required double? AverageRating { get; set; }
}