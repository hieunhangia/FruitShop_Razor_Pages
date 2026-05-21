namespace Service.DTOs.Admin.Account;

public class CreateStaffAccountDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required List<string> SelectedRoleNames { get; set; }
    public ShipperDataDto? ShipperData { get; set; }
}