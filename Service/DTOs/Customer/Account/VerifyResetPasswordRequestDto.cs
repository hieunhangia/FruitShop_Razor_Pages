namespace Service.DTOs.Customer.Account;

public class VerifyResetPasswordRequestDto
{
    public required string UserId { get; set; }
    public required string Code { get; set; }
}