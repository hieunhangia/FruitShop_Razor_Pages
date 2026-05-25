namespace Service.DTOs.Everyone.Product;

public class ProductSummaryDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required long Price { get; set; }
    public required string ImageFilePath { get; set; }
    public string? ImageFileUrl { get; set; }
    public required string ProductUnitName { get; set; }
    public required double? AverageRating { get; set; }
}