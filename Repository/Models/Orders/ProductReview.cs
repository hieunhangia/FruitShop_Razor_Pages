using System.ComponentModel.DataAnnotations;
using Repository.Models.Products;
using Repository.Models.Users;

namespace Repository.Models.Orders;

public class ProductReview
{
    public long OrderId { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public OrderItem? OrderItem { get; set; }

    [Required]
    [Range(BusinessRuleConstants.Model.ProductReview.RatingMinValue,
        BusinessRuleConstants.Model.ProductReview.RatingMaxValue)]
    public required int Rating { get; set; }

    [MaxLength(BusinessRuleConstants.Model.ProductReview.CommentMaxLength)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required] public bool IsResolved { get; set; }

    [MaxLength(BusinessRuleConstants.Model.ProductReview.ResolutionMessageMaxLength)]
    public string? ResolutionMessage { get; set; }

    public DateTime? ResolvedAt { get; set; }
    public int? ResolvedByCustomerSupportId { get; set; }
    public CustomerSupportData? ResolvedByCustomerSupport { get; set; }
}