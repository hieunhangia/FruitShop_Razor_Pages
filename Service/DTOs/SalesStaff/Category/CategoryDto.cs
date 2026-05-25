using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.SalesStaff.Category
{
    public class CategoryDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
