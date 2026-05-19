using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.Shipper
{
    public class OrderFilterDto
    {
        public string? SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PaymentMethod { get; set; }
    }
}
