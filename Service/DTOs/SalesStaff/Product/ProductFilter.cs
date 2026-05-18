using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.SalesStaff.Product
{
    public class ProductFilter
    {
        public string? SearchName { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}
