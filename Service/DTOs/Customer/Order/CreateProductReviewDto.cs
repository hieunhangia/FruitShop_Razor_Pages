using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.Customer.Order
{
    public class CreateProductReviewDto
    {
        public long OrderId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
