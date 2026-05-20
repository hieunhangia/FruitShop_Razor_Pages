namespace Service.DTOs.Admin.Account;

public class AccountFilter
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public bool? EmailConfirmed { get; set; }
    public bool? IsLockedOut { get; set; }
}