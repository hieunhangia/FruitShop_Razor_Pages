using System;
using System.Collections.Generic;
using System.Text;
using Repository.Models.Orders;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.Order
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ReviewMapper
    {
        public partial ProductReview ToProductReview(CreateProductReviewDto dto);
    
    }
}
