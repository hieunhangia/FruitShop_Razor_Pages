using ElmahCore.Mvc;
using ElmahCore.Postgresql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Minio;
using PayOS;
using Repository;
using Repository.Constants;
using Service.Admin.HealthCheck;

namespace Service;

public static class DependencyInjection
{
    public static void AddServiceLevelServices(this IHostApplicationBuilder builder)
    {
        // Google OpenId Connect
        builder.Services.AddAuthentication().AddGoogleOpenIdConnect(googleOptions =>
        {
            googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        });

        // Minio client and file service
        var minioSettings = builder.Configuration.GetSection("MinioSettings");
        builder.Services.AddMinio(configureClient => configureClient
            .WithEndpoint(minioSettings["Endpoint"])
            .WithCredentials(minioSettings["AccessKey"], minioSettings["SecretKey"])
            .WithSSL(Convert.ToBoolean(minioSettings["UseSSL"]))
            .Build());
        builder.Services.AddSingleton<FileService>();

        // Email service
        builder.Services.AddSingleton<EmailService>();

        // PayOS client
        builder.Services.AddSingleton(new PayOSClient(new PayOSOptions
        {
            ClientId = builder.Configuration["PayOS:ClientId"],
            ApiKey = builder.Configuration["PayOS:ApiKey"],
            ChecksumKey = builder.Configuration["PayOS:ChecksumKey"]
        }));


        // Health checks
        builder.Services.AddHttpClient();
        builder.Services.AddHealthChecks()
            .AddCheck<UptimeHealthCheck>("Tình trạng hoạt động")
            .AddCheck("Phân vùng đĩa",
                new DiskPartitionHealthCheck(BusinessRuleConstants.HealthCheck.MinimumFreeSpaceUnhealthyGB))
            .AddCheck("Bộ nhớ RAM", new RamHealthCheck(BusinessRuleConstants.HealthCheck.MaximumRamUsageUnhealthyMB))
            .AddCheck<PayOsHealthCheck>("PayOS API")
            .AddCheck<MinioHealthCheck>("Minio");
        builder.Services.AddSingleton<HealthCheckService>();

        // ELMAH
        builder.Services.AddElmah<PgsqlErrorLog>(options =>
        {
            options.Path = BusinessRuleConstants.AdminPageRoute.ErrorLogPage;
            options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            options.OnPermissionCheck =
                context => context.User.Identity is { IsAuthenticated: true } && context.User.IsInRole(Role.Admin);
        });
    }
}