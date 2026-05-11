using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Repository.Models.Products;

namespace Repository.Models.Orders;

public class OrderItem
{
    [Required] public int OrderId { get; set; }
    public Order? Order { get; set; }

    [Required] public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    [Column(TypeName = "jsonb")]
    public required string ProductSnapshot { get; set; }

    [Required] public required int Quantity { get; set; }

    [Required] public required long UnitPrice { get; set; }
}

public class ProductSnapshot
{
    public required string Name { get; set; }
    public required string ImageUrl { get; set; }
    public required string ProductUnit { get; set; }

    public static ProductSnapshot FromProduct(Product product) => new()
    {
        Name = product.Name,
        ImageUrl = product.ImageUrl,
        ProductUnit = product.ProductUnit?.Name ?? string.Empty,
    };

    public static ProductSnapshot? FromJson(string json) => JsonSerializer.Deserialize<ProductSnapshot>(json);

    public string ToJson() => JsonSerializer.Serialize(this);
}