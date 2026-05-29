using System;
using System.Collections.Generic;
using System.Text;
using Repository.Constants;

namespace Service.DTOs.CustomerSupport
{
    public class CommentFilterDto
    {
        public string? SearchContent { get; set; } 
        public CommentClassification? CommentClassification { get; set; }
        public int? Rating { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
