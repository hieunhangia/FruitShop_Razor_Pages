using Repository.Models.Orders;

namespace Repository.Models.Users;

public class CustomerData
{
    public int CustomerId { get; set; }
    public User? Customer { get; set; }
    public long LoyaltyPoints { get; set; }

    public ICollection<CartItem>? CartItems { get; set; }
    public ICollection<ShippingAddress>? ShippingAddresses { get; set; }
}