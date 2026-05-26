namespace Service.DTOs.Customer.Account;

public class ConfirmEmailDto
{
    public required string UserId { get; set; }
    public required string Code { get; set; }
}