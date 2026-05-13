using System.ComponentModel.DataAnnotations;

namespace Repository.Models.Users;

public class ShipperInformation
{
    public int ShipperId { get; set; }
    public User? Shipper { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.ShipperInfomation.ShipperNameMaxLength)]
    public required string ShipperName { get; set; }
}