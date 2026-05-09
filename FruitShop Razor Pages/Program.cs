using FluentEmail.MailKitSmtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Identity;
using Repository.Models.Users;
using Service;
using Service.Customer;
using Service.DTOs.Address;
using Service.DTOs.Customer.Cart;
using Service.DTOs.Customer.ShippingAddress;

var builder = WebApplication.CreateBuilder(args);

// Add secret configuration from appsettings.secret.json
builder.Configuration.AddJsonFile("appsettings.secret.json", false);

// Add PostgreSQL database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
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

// Add FluentEmail services
var mailSettings = builder.Configuration.GetSection("MailSettings");
builder.Services
    .AddFluentEmail(mailSettings["EmailAddress"], mailSettings["EmailDisplayName"])
    .AddRazorRenderer()
    .AddMailKitSender(new SmtpClientOptions
    {
        Server = mailSettings["SmtpServer"],
        Port = int.Parse(mailSettings["SmtpPort"]!),
        User = mailSettings["SmtpUser"],
        Password = mailSettings["SmtpPassword"],
        RequiresAuthentication = true
    });
builder.Services.AddTransient<EmailService>();

AddMappers();

AddApplicationServices();

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

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();
app.MapControllers()
    .WithStaticAssets();

app.Run();
return;

void AddMappers()
{
    builder.Services.AddSingleton<AddressMapper>();
    builder.Services.AddSingleton<ShippingAddressMapper>();
    builder.Services.AddSingleton<CartMapper>();
}

void AddApplicationServices()
{
    builder.Services.AddScoped<AddressService>();
    builder.Services.AddScoped<ShippingAddressService>();
    builder.Services.AddScoped<CartService>();
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
            Role.AllRoles.ToArray()
        ),
        (
            "manager@app.com",
            "Manager@123",
            [Role.Manager]
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
            Email = userData.Email
        };
        await userManager.CreateAsync(user, userData.Password);
        await userManager.ConfirmEmailAsync(user, await userManager.GenerateEmailConfirmationTokenAsync(user));
        await userManager.AddToRolesAsync(user, userData.Roles);
    }

    dbContext.Database.ExecuteSqlRaw(File.ReadAllText("sample data.sql"));

    var customer1 = userManager.Users.FirstOrDefault(u => u.Email == "customer1@app.com")!;
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
        RecipientName = "hieunhangia",
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
        SpecificAddress = "Đường Tàu",
        CustomerId = customer1.Id
    };
    var caoBangShippingAddress = new ShippingAddress
    {
        RecipientName = "Trà Từ Tay",
        RecipientPhoneNumber = "0123456789",
        CommuneCode = "01279",
        SpecificAddress = "136 An Liễng",
        CustomerId = customer1.Id
    };
    dbContext.ShippingAddresses.AddRange(bacNinhShippingAddress, haNoiShippingAddress, thanhHoaShippingAddress,
        caoBangShippingAddress);
    await dbContext.SaveChangesAsync();
}