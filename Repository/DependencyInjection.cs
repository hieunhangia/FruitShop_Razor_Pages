using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository.Constants;
using Repository.Identity;
using Repository.Models.Users;

namespace Repository;

public static class DependencyInjection
{
    public static void AddRepositoryLevelServices(this IHostApplicationBuilder builder)
    {
        // PostgreSQL database context
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Identity services
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy => { policy.RequireRole(Role.Admin); });
        builder.Services.AddTransient<CustomEmailConfirmationTokenProvider>();
        builder.Services.AddTransient<CustomPasswordResetTokenProvider>();
        builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = BusinessRuleConstants.Identity.Password.RequiredLength;
                options.Password.RequireDigit = BusinessRuleConstants.Identity.Password.RequireDigit;
                options.Password.RequireLowercase = BusinessRuleConstants.Identity.Password.RequireLowercase;
                options.Password.RequireUppercase = BusinessRuleConstants.Identity.Password.RequireUppercase;
                options.Password.RequireNonAlphanumeric =
                    BusinessRuleConstants.Identity.Password.RequireNonAlphanumeric;
                options.Password.RequiredUniqueChars = BusinessRuleConstants.Identity.Password.RequiredUniqueChars;

                options.SignIn.RequireConfirmedEmail = true;

                options.User.RequireUniqueEmail = true;

                options.Lockout.DefaultLockoutTimeSpan =
                    TimeSpan.FromMinutes(BusinessRuleConstants.Identity.Lockout.LockoutMinutes);
                options.Lockout.MaxFailedAccessAttempts =
                    BusinessRuleConstants.Identity.Lockout.MaxFailedAccessAttempts;

                options.Tokens.ProviderMap.Add("CustomEmailConfirmation",
                    new TokenProviderDescriptor(typeof(CustomEmailConfirmationTokenProvider)));
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                options.Tokens.ProviderMap.Add("CustomPasswordReset",
                    new TokenProviderDescriptor(typeof(CustomPasswordResetTokenProvider)));
                options.Tokens.PasswordResetTokenProvider = "CustomPasswordReset";
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddErrorDescriber<VietnameseIdentityErrorDescriber>();
    }
}