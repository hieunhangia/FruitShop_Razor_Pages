namespace Service.DTOs.Everyone.ProductReview;

public class ProductReviewDto
{
    public required string ReviewerEmail { get; set; }
    public required int Rating { get; set; }
    public string? Comment { get; set; }
    public required DateTime CreatedAt { get; set; }
    public string? ResolutionMessage { get; set; }
}