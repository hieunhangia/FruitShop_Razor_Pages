using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DTOs.CustomerSupport
{
    public class CommentSummaryDto
    {
        public long OrderId { get; set; } 
        public int ProductId { get; set; } 
        public int Rating { get; set; } 
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string CommentClassification { get; set; }
    }
}
