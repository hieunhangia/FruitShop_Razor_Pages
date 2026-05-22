using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Repository.Models.Orders;

namespace Repository.Models.Products;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(DisplayOrder), IsUnique = true)]
public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.Product.NameMaxLength)]
    public required string Name { get; set; }

    [Required] public required string Description { get; set; }

    [Required] public required long Price { get; set; }

    [Required] public required int Quantity { get; set; }

    [Required] public int HeldQuantity { get; set; }

    [Required] public required string ImageFilePath { get; set; }

    [Required] public bool IsActive { get; set; } = true;
    [Required] public required int DisplayOrder { get; set; }

    [Required] public int ProductUnitId { get; set; }
    public ProductUnit? ProductUnit { get; set; }

    public ICollection<Category>? Categories { get; set; }

    public ICollection<ProductReview>? ProductReviews { get; set; }
}