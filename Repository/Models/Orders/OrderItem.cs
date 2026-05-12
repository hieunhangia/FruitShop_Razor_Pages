using System.ComponentModel.DataAnnotations;
using Repository.Models.Products;

namespace Repository.Models.Orders;

public class OrderItem
{
    [Required] public long OrderId { get; set; }
    public Order? Order { get; set; }

    [Required] public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    public required ProductSnapshot ProductSnapshot { get; set; }

    [Required] public required int Quantity { get; set; }
}

public class ProductSnapshot
{
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
    public required string ProductUnitName { get; set; }
    public required long UnitPrice { get; set; }
}