using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.ProductReview;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ProductReviewMapper
{
    public static partial Repository.Models.Orders.ProductReview ToProductReview(CreateProductReviewDto dto);
}