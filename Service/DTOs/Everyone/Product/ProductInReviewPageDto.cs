namespace Service.DTOs.Everyone.Product;

public class ProductInReviewPageDto
{
    public required string Name { get; set; }
    public required string ImageFilePath { get; set; }
    public string? ImageFileUrl { get; set; }
    public required int ProductReviewsCount { get; set; }
    public required double? AverageRating { get; set; }
    public PagedAndSortedDto<ProductReviewDto>? ProductReviews { get; set; }
}