using Repository.Models.Users;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Admin.Account;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AccountMapper
{
    [MapperIgnoreTarget(nameof(AccountDto.Roles))]
    public partial AccountDto ToAccountDto(User user);
    
    public partial List<AccountDto> ToAccountDtoList(List<User> users);
}