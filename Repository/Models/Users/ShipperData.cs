using System.ComponentModel.DataAnnotations;
using Repository.Models.Orders;

namespace Repository.Models.Users;

public class ShipperData
{
    public int ShipperId { get; set; }
    public User? Shipper { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.ShipperData.NameMaxLength)]
    public required string ShipperName { get; set; }
    
    public ICollection<Order>? Orders { get; set; }
}