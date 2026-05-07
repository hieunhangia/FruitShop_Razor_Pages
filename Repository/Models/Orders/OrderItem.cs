using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Models.Products;

namespace Repository.Models.Orders;

public class OrderItem
{
    [Required] public int OrderId { get; set; }
    public Orders.Order? Order { get; set; }

    [Required] public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    [Column(TypeName = "jsonb")]
    public required string ProductSnapshot { get; set; }

    [Required] public required int Quantity { get; set; }

    [Required] public required long UnitPrice { get; set; }
}