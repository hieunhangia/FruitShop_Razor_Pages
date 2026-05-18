using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.SalesStaff.Product
{
    public class CreateProductDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public int DisplayOrder { get; set; }
        public int ProductUnitId { get; set; }
        public List<int> CategoryIds { get; set; } = [];
    }
}
