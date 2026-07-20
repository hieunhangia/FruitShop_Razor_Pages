using System;
using System.Collections.Generic;
using System.Text;
using Repository.Constants;

namespace Service.DTOs.CustomerSupport
{
    public class CommentDetailDto
    {
        public long OrderId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public CommentClassification CommentClassification { get; set; }

        public string? ResolutionMessage { get; set; }
    }
}
