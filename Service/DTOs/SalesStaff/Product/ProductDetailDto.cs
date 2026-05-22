using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.SalesStaff.Product
{
    public class ProductDetailDto : ProductSummaryDto
    {
        public required string Description { get; set; }
        public int HeldQuantity { get; set; }
        public required int DisplayOrder { get; set; }
        public required List<int> CategoryIds { get; set; }

        public int ProductUnitId { get; set; }
        public List<string>? CategoryNames { get; set; }  
    }
}
