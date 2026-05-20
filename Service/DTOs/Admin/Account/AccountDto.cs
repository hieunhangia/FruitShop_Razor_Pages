namespace Service.DTOs.Admin.Account;

public class AccountDto
{
    public required int Id { get; set; }
    public required string Email { get; set; }
    public required bool EmailConfirmed { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public List<string>? Roles;
}