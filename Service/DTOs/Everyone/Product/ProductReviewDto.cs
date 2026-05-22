namespace Service.DTOs.Everyone.Product;

public class ProductReviewDto
{
    public required int Rating { get; set; }
    public string? Comment { get; set; }
    public required DateTime CreatedAt { get; set; }
    public string? ResolutionMessage { get; set; }
}