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
    private static string MapReviewerEmail(string email)
    {
        if (!email.Contains('@'))
        {
            return email;
        }

        var parts = email.Split('@');
        var username = parts[0];
        var maskedUsername = username.Length > 3 ? username[..3] : username[..1];
        return $"{maskedUsername}***@{parts[1]}";
    }


    public partial List<ProductReviewDto> ToProductReviewDtoList(
        List<Repository.Models.Orders.ProductReview> productReviews);
}