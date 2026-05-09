namespace Service.DTOs.Customer.ShippingAddress;

public class AddShippingAddressDto
{
    public required string RecipientName { get; set; }
    public required string RecipientPhoneNumber { get; set; }
    public required string CommuneCode { get; set; }
    public required string SpecificAddress { get; set; }
}