using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Address;

[Index(nameof(Name), IsUnique = true)]
public class Province
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [MaxLength(BusinessRuleConstants.Model.Province.CodeMaxLength)]
    public required string Code { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.Province.NameMaxLength)]
    public required string Name { get; set; }

    public ICollection<Commune>? Communes { get; set; }
}