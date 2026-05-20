using Repository.Models.Orders;

namespace Repository.Models.Users;

public class CustomerSupportData
{
    public int CustomerSupportId { get; set; }
    public User? CustomerSupport { get; set; }

    public ICollection<ProductReview>? ProductReviews { get; set; }
}