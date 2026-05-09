using System.ComponentModel.DataAnnotations;
using Repository.Models.Address;

namespace Repository.Models.Users;

public class ShippingAddress
{
    public int Id { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.ShippingAddress.RecipientNameMaxLength)]
    public required string RecipientName { get; set; }

    [Required] public required string RecipientPhoneNumber { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.Commune.CodeMaxLength)]
    public required string CommuneCode { get; set; }

    public Commune? Commune { get; set; }

    [Required]
    [MaxLength(BusinessRuleConstants.Model.ShippingAddress.SpecificAddressMaxLength)]
    public required string SpecificAddress { get; set; }

    [Required] public bool IsDefault { get; set; }

    public int CustomerId { get; set; }
    public User? Customer { get; set; }
}