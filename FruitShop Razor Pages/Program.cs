using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Models.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.secret.json", false);

// Add PostgreSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
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

app.Run();
return;


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
            Item1 = "customer" + i + "@app.com",
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
}