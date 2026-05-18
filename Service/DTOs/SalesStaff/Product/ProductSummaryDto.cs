using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.SalesStaff.Product
{
    public class ProductSummaryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required long Price { get; set; }
        public required int Quantity { get; set; }
        public required string ImageFilePath { get; set; }
        public bool IsActive { get; set; }
        public required string ProductUnitName { get; set; }
    }
}
