using Service.DTOs.Everyone.Category;

namespace Service.DTOs.Everyone.Product;

public class ProductDetailDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }
    public required long Price { get; set; }
    public required int Quantity { get; set; }
    public required bool IsActive { get; set; }
    public required string ProductUnitName { get; set; }
    public required List<CategoryDto> Categories { get; set; }
    public required List<ProductReviewDto> ProductReviews { get; set; }
}