using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.SalesStaff.Category
{
    public class CategoryFilter
    {
        public string? SearchName { get; set; }
        public bool? IsActive { get; set; }
    }
}
