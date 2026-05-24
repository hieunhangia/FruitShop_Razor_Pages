using Repository;
using Repository.Models.Users;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Everyone.ProductReview;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductReviewMapper
{
    [MapProperty(
        $"{nameof(Repository.Models.Orders.ProductReview.Customer)}.{nameof(CustomerData.Customer)}.{nameof(User.Email)}",
        nameof(ProductReviewDto.ReviewerEmail), Use = nameof(MapReviewerEmail))]
    private partial ProductReviewDto ToProductReviewDto(Repository.Models.Orders.ProductReview productReview);

    [UserMapping(Default = false)]
    private static string MapReviewerEmail(string email) => BusinessRuleConstants.ProductReview.HideEmailAddress(email);

    public partial List<ProductReviewDto> ToProductReviewDtoList(
        List<Repository.Models.Orders.ProductReview> productReviews);
}