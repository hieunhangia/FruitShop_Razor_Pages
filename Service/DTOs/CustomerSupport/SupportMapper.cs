using System.Collections.Generic;
using Repository.Models.Orders;
using Riok.Mapperly.Abstractions;
using Service.DTOs.CustomerSupport;

namespace Service.DTOs.CustomerSupport;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class SupportMapper
{
    public partial List<CommentSummaryDto> ToCommentSummaryDtoList(List<ProductReview> reviews);

    [MapProperty($"{nameof(ProductReview.Customer)}.{nameof(Repository.Models.Users.CustomerData.Customer)}.{nameof(Repository.Models.Users.User.UserName)}",
        nameof(CommentSummaryDto.CustomerName))]
    public partial CommentSummaryDto ToCommentSummaryDto(ProductReview review);
}