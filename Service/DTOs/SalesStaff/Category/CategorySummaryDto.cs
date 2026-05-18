using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.SalesStaff.Category
{
    public class CategorySummaryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
    }
}
