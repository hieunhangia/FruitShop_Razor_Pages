using ElmahCore.Mvc;
using ElmahCore.Postgresql;
using FruitShop_Razor_Pages.BackgroundService;
using FruitShop_Razor_Pages.Extensions;
using Markdig;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Minio;
using PayOS;
using Repository;
using Repository.Constants;
using Repository.Identity;
using Repository.Models.Users;
using Service;
using Service.Admin;
using Service.Admin.HealthCheck;
using Service.Customer;
using Service.SalesStaff;
using Service.DTOs.Address;
using Service.DTOs.Admin.Account;
using Service.DTOs.Admin.HealthCheck;
using Service.DTOs.Customer.Cart;
using Service.DTOs.Customer.Coupon;
using Service.DTOs.Customer.Order;
using Service.DTOs.Customer.ShippingAddress;
using Service.DTOs.Everyone.Category;
using Service.DTOs.Everyone.Product;
using HealthCheckService = Service.Admin.HealthCheck.HealthCheckService;

var builder = WebApplication.CreateBuilder(args);

// Add secret configuration from appsettings.secret.json
builder.Configuration.AddJsonFile("appsettings.secret.json", false);

// Add PostgreSQL database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
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
        options.Password.RequireNonAlphanumeric = BusinessRuleConstants.Identity.Password.RequireNonAlphanumeric;
        options.Password.RequiredUniqueChars = BusinessRuleConstants.Identity.Password.RequiredUniqueChars;

        options.SignIn.RequireConfirmedEmail = true;

        options.User.RequireUniqueEmail = true;

        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(BusinessRuleConstants.Identity.Lockout.LockoutMinutes);
        options.Lockout.MaxFailedAccessAttempts = BusinessRuleConstants.Identity.Lockout.MaxFailedAccessAttempts;

        options.Tokens.ProviderMap.Add("CustomEmailConfirmation",
            new TokenProviderDescriptor(typeof(CustomEmailConfirmationTokenProvider)));
        options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

        options.Tokens.ProviderMap.Add("CustomPasswordReset",
            new TokenProviderDescriptor(typeof(CustomPasswordResetTokenProvider)));
        options.Tokens.PasswordResetTokenProvider = "CustomPasswordReset";
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddErrorDescriber<VietnameseIdentityErrorDescriber>();

// Add Minio client and file service
var minioSettings = builder.Configuration.GetSection("MinioSettings");
builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(minioSettings["Endpoint"])
    .WithCredentials(minioSettings["AccessKey"], minioSettings["SecretKey"])
    .WithSSL(Convert.ToBoolean(minioSettings["UseSSL"]))
    .Build());
builder.Services.AddSingleton<FileService>();

// Add email service
builder.Services.AddSingleton<EmailService>();

// Add PayOS client
builder.Services.AddSingleton(new PayOSClient(new PayOSOptions
{
    ClientId = builder.Configuration["PayOS:ClientId"],
    ApiKey = builder.Configuration["PayOS:ApiKey"],
    ChecksumKey = builder.Configuration["PayOS:ChecksumKey"]
}));

builder.Services.AddHttpClient();
builder.Services.AddHealthChecks()
    .AddCheck<UptimeHealthCheck>("Tình trạng hoạt động")
    .AddCheck("Phân vùng đĩa",
        new DiskPartitionHealthCheck(BusinessRuleConstants.HealthCheck.MinimumFreeSpaceUnhealthyGB))
    .AddCheck("Bộ nhớ RAM", new RamHealthCheck(BusinessRuleConstants.HealthCheck.MaximumRamUsageUnhealthyMB))
    .AddCheck<PayOsHealthCheck>("PayOS API")
    .AddCheck<MinioHealthCheck>("Minio");
builder.Services.AddSingleton<HealthCheckService>();
builder.Services.AddElmah<PgsqlErrorLog>(options =>
{
    options.Path = BusinessRuleConstants.AdminPageRoute.ErrorLogPage;
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.OnPermissionCheck =
        context => context.User.IsAuthenticated() && context.User.IsInRole(Role.Admin);
});


// Add Markdig Markdown pipeline with custom image rendering
builder.Services.AddKeyedSingleton("CustomImage", new MarkdownPipelineBuilder()
    .UseAdvancedExtensions()
    .Use(new CustomImageMarkdigExtensions())
    .Build());

AddMappers();

AddApplicationServices();

AddHostedService();

// Add Razor Pages services
builder.Services.AddRazorPages();

var app = builder.Build();

await SeedDataAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseElmah();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();
app.MapControllers()
    .WithStaticAssets();
app.MapHealthChecks(BusinessRuleConstants.HealthCheck.HealthCheckApi, new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new HealthCheckDto
            {
                Status = report.Status,
                Items = report.Entries.Select(entry => new HealthCheckItemDto
                {
                    Name = entry.Key,
                    Status = entry.Value.Status,
                    Description = entry.Value.Description,
                    Data = entry.Value.Data,
                    ExceptionStackTrace = entry.Value.Exception?.StackTrace
                }).ToList()
            });
        }
    })
    .RequireAuthorization("RequireAdminRole");

app.Run();
return;

void AddMappers()
{
    builder.Services.AddSingleton<AddressMapper>();
    builder.Services.AddSingleton<ShippingAddressMapper>();
    builder.Services.AddSingleton<CartMapper>();
    builder.Services.AddSingleton<OrderMapper>();
    builder.Services.AddSingleton<CouponMapper>();
    builder.Services.AddSingleton<ProductMapper>();
    builder.Services.AddSingleton<CategoryMapper>();
    builder.Services.AddSingleton<Service.DTOs.Manager.CouponMapper>();
    builder.Services.AddSingleton<AccountMapper>();
    builder.Services.AddSingleton<ReviewMapper>();
}

void AddApplicationServices()
{
    builder.Services.AddScoped<AddressService>();
    builder.Services.AddScoped<ShippingAddressService>();
    builder.Services.AddScoped<CartService>();
    builder.Services.AddScoped<OrderService>();
    builder.Services.AddScoped<CouponService>();
    builder.Services.AddScoped<Service.Everyone.ProductService>();
    builder.Services.AddScoped<Service.Everyone.CategoryService>();
    builder.Services.AddScoped<ProductService>();
    builder.Services.AddScoped<CategoryService>();
    builder.Services.AddScoped<Service.Manager.CouponService>();
    builder.Services.AddScoped<Service.Shipper.OrderService>();
    builder.Services.AddScoped<AccountService>();
    builder.Services.AddScoped<ProductReviewService>();
}

void AddHostedService()
{
    builder.Services.AddHostedService<CancelExpiredQrCodePaymentOrderBackgroundService>();
}

async Task SeedDataAsync()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureDeletedAsync();
    await dbContext.Database.EnsureCreatedAsync();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

    foreach (var roleName in Role.AllRoles)
    {
        await roleManager.CreateAsync(new IdentityRole<int>(roleName));
    }

    ICollection<(string Email, string Password, string[] Roles)> users =
    [
        (
            "admin@app.com",
            "Admin@123",
            Role.AdminRoles
        ),
        (
            "manager@app.com",
            "Manager@123",
            Role.ManagerRoles
        )
    ];
    for (var i = 1; i <= 5; i++)
    {
        users.Add(new ValueTuple<string, string, string[]>
        {
            Item1 = $"sales-staff{i}@app.com",
            Item2 = "SalesStaff@123",
            Item3 = [Role.SalesStaff]
        });

        users.Add(new ValueTuple<string, string, string[]>
        {
            Item1 = $"shipper{i}@app.com",
            Item2 = "Shipper@123",
            Item3 = [Role.Shipper]
        });

        users.Add(new ValueTuple<string, string, string[]>
        {
            Item1 = $"customer-support{i}@app.com",
            Item2 = "CustomerSupport@123",
            Item3 = [Role.CustomerSupport]
        });

        users.Add(new ValueTuple<string, string, string[]>
        {
            Item1 = $"customer{i}@app.com",
            Item2 = "Customer@123",
            Item3 = [Role.Customer]
        });
    }

    foreach (var userData in users)
    {
        var user = new User
        {
            UserName = userData.Email,
            Email = userData.Email,
            EmailConfirmed = true
        };
        if (userData.Roles.Contains(Role.Customer))
        {
            user.CustomerData = new CustomerData
            {
                LoyaltyPoints = Random.Shared.NextInt64(10000, 50000)
            };
        }

        if (userData.Roles.Contains(Role.CustomerSupport))
        {
            user.CustomerSupportData = new CustomerSupportData();
        }

        await userManager.CreateAsync(user, userData.Password);
        await userManager.AddToRolesAsync(user, userData.Roles);
    }

    dbContext.Database.ExecuteSqlRaw(File.ReadAllText("sample data/address.sql"));
    dbContext.Database.ExecuteSqlRaw(File.ReadAllText("sample data/coupon.sql"));
    dbContext.Database.ExecuteSqlRaw(File.ReadAllText("sample data/product.sql"));

    var customer1 = userManager.Users.AsNoTracking().FirstOrDefault(u => u.Email == "customer1@app.com")!;
    var bacNinhShippingAddress = new ShippingAddress
    {
        RecipientName = "hieunhangia",
        RecipientPhoneNumber = "0888888888",
        CommuneCode = "09469",
        SpecificAddress = "Bắc Bling",
        CustomerId = customer1.Id
    };
    var haNoiShippingAddress = new ShippingAddress
    {
        RecipientName = "scammerfpt",
        RecipientPhoneNumber = "0777777777",
        CommuneCode = "09955",
        SpecificAddress = "Trường Đại học FPT",
        CustomerId = customer1.Id,
        IsDefault = true
    };
    var thanhHoaShippingAddress = new ShippingAddress
    {
        RecipientName = "Hóa Thanh Sư",
        RecipientPhoneNumber = "0363636363",
        CommuneCode = "16279",
        SpecificAddress = "Quốc Lộ 36",
        CustomerId = customer1.Id
    };
    var caoBangShippingAddress = new ShippingAddress
    {
        RecipientName = "Tay Trừ Tà",
        RecipientPhoneNumber = "0123456789",
        CommuneCode = "01279",
        SpecificAddress = "136 An Liễng",
        CustomerId = customer1.Id
    };
    dbContext.ShippingAddresses.AddRange(bacNinhShippingAddress, haNoiShippingAddress, thanhHoaShippingAddress,
        caoBangShippingAddress);

    var shippers = await dbContext.Users.Where(u => u.Email!.Contains("shipper")).ToListAsync();
    shippers[0].PhoneNumber = "0000000000";
    shippers[0].ShipperData = new ShipperData
    {
        ShipperName = "Doraemon"
    };
    shippers[1].PhoneNumber = "0000000001";
    shippers[1].ShipperData = new ShipperData
    {
        ShipperName = "Son Goku"
    };
    shippers[2].PhoneNumber = "0000000002";
    shippers[2].ShipperData = new ShipperData
    {
        ShipperName = "Monkey D. Luffy"
    };
    shippers[3].PhoneNumber = "0000000003";
    shippers[3].ShipperData = new ShipperData
    {
        ShipperName = "Uzumaki Naruto"
    };
    shippers[4].PhoneNumber = "0000000004";
    shippers[4].ShipperData = new ShipperData
    {
        ShipperName = "Edogawa Conan"
    };

    await dbContext.SaveChangesAsync();

    await using var connection = dbContext.Database.GetDbConnection();
    await using var command = connection.CreateCommand();
    command.CommandText = File.ReadAllText("sample data/order.sql");
    await dbContext.Database.OpenConnectionAsync();
    await command.ExecuteNonQueryAsync();
}