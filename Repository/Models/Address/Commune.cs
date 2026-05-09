using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models.Address;

[Index(nameof(Name), IsUnique = true)]
public class Commune
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [MaxLength(BusinessRuleConstants.Model.Commune.CodeMaxLength)]
    public required string Code { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.Commune.NameMaxLength)]
    public required string Name { get; set; }

    public Province? Province { get; set; }
}