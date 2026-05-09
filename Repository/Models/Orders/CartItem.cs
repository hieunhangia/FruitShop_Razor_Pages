using System.ComponentModel.DataAnnotations;
using Repository.Models.Products;
using Repository.Models.Users;

namespace Repository.Models.Orders;

public class CartItem
{
    [Required] public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required] public int CustomerId { get; set; }
    public User? Customer { get; set; }

    [Required] public required int Quantity { get; set; }

    [Required] public bool IsSelected { get; set; }
}