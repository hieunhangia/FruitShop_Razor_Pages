using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models.Address;

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

    [Required]
    [MaxLength(BusinessRuleConstants.Model.Province.CodeMaxLength)]
    public string ProvinceCode { get; set; } = null!;

    public Province? Province { get; set; }
}