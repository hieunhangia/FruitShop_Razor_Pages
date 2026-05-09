namespace Service.DTOs.Customer.ShippingAddress;

public class ShippingAddressDto
{
    public required int Id { get; set; }
    public required string RecipientName { get; set; }
    public required string RecipientPhoneNumber { get; set; }
    public required string CommuneCode { get; set; }
    public required string CommuneName { get; set; }
    public required string ProvinceCode { get; set; }
    public required string ProvinceName { get; set; }
    public required string SpecificAddress { get; set; }
    public required bool IsDefault { get; set; }
}