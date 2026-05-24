using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Service.DTOs.Customer.ProductReview;
using ProductReviewMapper = Service.DTOs.Customer.ProductReview.ProductReviewMapper;

namespace Service.Customer;

public class ProductReviewService(AppDbContext context)
{
    public async Task<bool> CanReviewAsync(long orderId, int productId, int customerId)
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

    public async Task CreateReviewAsync(CreateProductReviewDto dto, int customerId)
    {
        var review = ProductReviewMapper.ToProductReview(dto);
        review.CreatedAt = DateTime.UtcNow;
        review.CommentClassification = CommentClassification.Unclassified;
        review.AssignedCustomerSupportId = await GetLeastBusyCustomerSupportIdAsync();
        review.CustomerId = customerId;

        context.ProductReviews.Add(review);
        await context.SaveChangesAsync();
    }

    private async Task<int> GetLeastBusyCustomerSupportIdAsync()
    {
        var leastBusyStaff = await context.CustomerSupportData
            .Select(s => new
            {
                staffId = s.CustomerSupportId,
                reviewCount = s.ProductReviews.Count(pr => pr.ResolvedAt == null)
            })
            .OrderBy(x => x.reviewCount)
            .FirstOrDefaultAsync();

        return leastBusyStaff.staffId;
    }
}