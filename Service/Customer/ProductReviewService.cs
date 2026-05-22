using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Models.Orders;
using Service.DTOs.Customer.Order;

namespace Service.Customer
{
    public class ProductReviewService(AppDbContext context, ReviewMapper mapper)
    {
        public async Task<bool> CanReviewAsync(long orderId,int productId, int customerId)
        {
            var isOrderValid = await context.Orders
                .AnyAsync(o => o.Id == orderId
                && o.CustomerId == customerId
                    && o.OrderStatus == OrderStatus.Delivered
                    && o.OrderItems.Any(oi => oi.ProductId == productId));
            if (!isOrderValid)
            {
                return false;
            }
            var isAlreadyReview = await context.ProductReviews
                .AnyAsync(pr => pr.OrderId == orderId
                && pr.ProductId == productId);

            return !isAlreadyReview;    
        }

        public async Task CreateReviewAsync(CreateProductReviewDto dto)
        {
                ProductReview review = mapper.ToProductReview(dto);
                review.CreatedAt = DateTime.UtcNow;
                review.CommentClassification = CommentClassification.Unclassified;
                review.AssignedCustomerSupportId = 1;

                context.ProductReviews.Add(review);
                await context.SaveChangesAsync();
         
        }
    }
}
