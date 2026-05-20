using System.ComponentModel.DataAnnotations;

namespace Repository.Models.Users;

public class ShipperData
{
    public int ShipperId { get; set; }
    public User? Shipper { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.ShipperData.NameMaxLength)]
    public required string ShipperName { get; set; }
}