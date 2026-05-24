using Repository.Models.Users;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Admin.Account;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class AccountMapper
{
    [MapperIgnoreTarget(nameof(AccountDto.Roles))]
    private static partial AccountDto ToAccountDto(User user);

    public static partial IQueryable<AccountDto> ProjectToAccountDto(this IQueryable<User> users);
}