using System.ComponentModel.DataAnnotations;
using Repository.Constants;
using Repository.Models.Products;
using Repository.Models.Users;

namespace Repository.Models.Orders;

public class ProductReview
{
    public long OrderId { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public OrderItem? OrderItem { get; set; }

    public int CustomerId { get; set; }
    public CustomerData? Customer { get; set; }

    [Required]
    [Range(BusinessRuleConstants.Model.ProductReview.RatingMinValue,
        BusinessRuleConstants.Model.ProductReview.RatingMaxValue)]
    public required int Rating { get; set; }

    [MaxLength(BusinessRuleConstants.Model.ProductReview.CommentMaxLength)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required] public CommentClassification CommentClassification { get; set; }
    public int AssignedCustomerSupportId { get; set; }
    public CustomerSupportData? AssignedCustomerSupport { get; set; }

    [MaxLength(BusinessRuleConstants.Model.ProductReview.ResolutionMessageMaxLength)]
    public string? ResolutionMessage { get; set; }

    public DateTime? ResolvedAt { get; set; }
}