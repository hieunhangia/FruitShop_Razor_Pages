namespace Service.DTOs.Admin.Account;

public class AccountFilter
{
    public string? SearchTerm { get; set; }
    public bool? EmailConfirmed { get; set; }
    public bool? IsLockedOut { get; set; }
}