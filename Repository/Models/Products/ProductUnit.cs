using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Products;

[Index(nameof(Name), IsUnique = true)]
public class ProductUnit
{
    public int Id { get; set; }

    [Required] [MaxLength(50)] public required string Name { get; set; }
}