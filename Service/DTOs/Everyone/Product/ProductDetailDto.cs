using Service.DTOs.Everyone.Category;

namespace Service.DTOs.Everyone.Product;

public class ProductDetailDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ImageFilePath { get; set; }
    public string? ImageFileUrl { get; set; }
    public required long Price { get; set; }
    public required int Quantity { get; set; }
    public int? QuantityInCart { get; set; }
    public required bool IsActive { get; set; }
    public required string ProductUnitName { get; set; }
    public required List<CategoryDto> ActiveCategories { get; set; }
    public List<ProductReviewDto>? TopProductReviews { get; set; }
    public required int ProductReviewsCount { get; set; }
    public required double? AverageRating { get; set; }
}