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
    public required string Code { get; set; }

    [Required] [MaxLength(50)] public required string Name { get; set; }

    public ICollection<Commune>? Communes { get; set; }
}