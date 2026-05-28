namespace Service.DTOs.Customer.Account;

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}