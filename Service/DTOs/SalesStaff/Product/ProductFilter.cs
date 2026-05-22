using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.SalesStaff.Product
{
    public class ProductFilter
    {
        public string? SearchName { get; set; }
        public long? PriceFrom { get; set; }
        public long? PriceTo { get; set; }
        public bool? IsActive { get; set; }
        public List<int> CategoryIds { get; set; } = [];
        public int? ProductUnitId { get; set; }
    }
}
