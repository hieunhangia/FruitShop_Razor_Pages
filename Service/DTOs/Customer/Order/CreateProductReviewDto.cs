using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Repository;

namespace Service.DTOs.Customer.Order
{
    public class CreateProductReviewDto
    {
        public long OrderId { get; set; }
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn số sao đánh giá!")]
        [Range(BusinessRuleConstants.Model.ProductReview.RatingMinValue,
           BusinessRuleConstants.Model.ProductReview.RatingMaxValue,
           ErrorMessage = "Số sao phải từ 1 đến 5!")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Vui lòng không bỏ trống!")]
        [StringLength(BusinessRuleConstants.Model.ProductReview.CommentMaxLength,
            ErrorMessage = "Vui lòng không comment quá 1000 ký tự!")]
        public string? Comment { get; set; }
    }
}
