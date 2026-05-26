namespace Service.DTOs.Everyone.Product;

public class ProductReviewDto
{
    public required int ProductId { get; set; }
    public required string ProductName { get; set; }
    public string? ReviewerEmail { get; set; }
    public required int Rating { get; set; }
    public required string? Comment { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string? ResolutionMessage { get; set; }
}