using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Products;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.Category.NameMaxLength)]
    public required string Name { get; set; }

    [Required] public bool IsActive { get; set; } = true;

    public ICollection<Product>? Products { get; set; }
}