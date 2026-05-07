using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Products;

[Index(nameof(Name), IsUnique = true)]
public class Product
{
    public int Id { get; set; }

    [Required] [MaxLength(50)] public required string Name { get; set; }

    [Required] public required string Description { get; set; }

    [Required] public required long Price { get; set; }

    [Required] public required int Quantity { get; set; }

    [Required] public int HeldQuantity { get; set; }

    [Required] public required string ImageUrl { get; set; }

    [Required] public bool IsActive { get; set; } = true;

    [Required] public required int ProductUnitId { get; set; }
    public ProductUnit? ProductUnit { get; set; }

    public ICollection<Category>? Categories { get; set; }
}