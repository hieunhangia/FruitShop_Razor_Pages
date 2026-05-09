using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repository.Models.Users;

namespace Repository.Identity;

public class CustomPasswordResetTokenProvider(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<PasswordResetTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<User>> logger)
    : DataProtectorTokenProvider<User>(dataProtectionProvider, options, logger);

public class PasswordResetTokenProviderOptions : DataProtectionTokenProviderOptions
{
    public PasswordResetTokenProviderOptions()
    {
        Name = "PasswordResetDataProtectorTokenProvider";
        TokenLifespan = TimeSpan.FromMinutes(30);
    }
}