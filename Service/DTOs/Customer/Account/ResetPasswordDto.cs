namespace Service.DTOs.Customer.Account;

public class ResetPasswordDto
{
    public required string UserId { get; set; }
    public required string Code { get; set; }
    public required string Password { get; set; }
}