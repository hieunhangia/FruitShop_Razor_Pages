namespace Service.DTOs.Customer.ShippingAddress;

public class UpdateShippingAddressDto
{
    public required int Id { get; set; }

    public required string RecipientName { get; set; }

    public required string RecipientPhoneNumber { get; set; }

    public required string CommuneCode { get; set; }

    public required string SpecificAddress { get; set; }
}