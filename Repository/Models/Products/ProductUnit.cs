using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Products;

[Index(nameof(Name), IsUnique = true)]
public class ProductUnit
{
    public int Id { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.ProductUnit.NameMaxLength)]
    public required string Name { get; set; }
}