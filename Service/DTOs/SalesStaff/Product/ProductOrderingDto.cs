using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.SalesStaff.Product
{
    public class ProductOrderingDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageFilePath { get; set; }
        public required bool IsActive { get; set; }
    }
}
