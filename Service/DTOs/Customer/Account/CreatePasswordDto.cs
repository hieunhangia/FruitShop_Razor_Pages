namespace Service.DTOs.Customer.Account;

public class CreatePasswordDto
{
    public required string UserId { get; set; }
    public required string Password { get; set; }
}