using System.Collections.Generic;
using Repository.Models.Orders;
using Riok.Mapperly.Abstractions;
using Service.DTOs.CustomerSupport;

namespace Service.DTOs.CustomerSupport;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class SupportMapper
{
    public partial List<CommentSummaryDto> ToCommentSummaryDtoList(List<ProductReview> reviews);

    public partial CommentSummaryDto ToCommentSummaryDto(ProductReview review);

   
    public partial CommentDetailDto ToCommentDetailDto(ProductReview review);
}