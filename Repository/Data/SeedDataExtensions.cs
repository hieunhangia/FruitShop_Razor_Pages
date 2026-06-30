using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Constants;
using BusinessRuleModel = Repository.Models.BusinessRule.BusinessRule;
using Repository.Models.Users;

namespace Repository.Data;

public static class SeedDataExtensions
{
    public static async Task SeedDataAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        foreach (var roleName in Role.AllRoles)
        {
            await roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }

        dbContext.BusinessRules.AddRange(
            new BusinessRuleModel { Type = BusinessRuleConstantType.PrivateFileUrlExpirationSeconds, Value = "900", Description = "Thời gian hết hạn URL file private (giây)" },
            new BusinessRuleModel { Type = BusinessRuleConstantType.QrCodePaymentOrderExpiredMinutes, Value = "5", Description = "Thời gian hết hạn đơn hàng thanh toán QR (phút)" },
            new BusinessRuleModel { Type = BusinessRuleConstantType.LoyaltyPointEarnedWhenRegister, Value = "300", Description = "Điểm thưởng khi đăng ký tài khoản" },
            new BusinessRuleModel { Type = BusinessRuleConstantType.LoyaltyPointEarnedPerComment, Value = "36", Description = "Điểm thưởng khi đánh giá sản phẩm" },
            new BusinessRuleModel { Type = BusinessRuleConstantType.VNDPerLoyaltyPoint, Value = "1000", Description = "Giá trị quy đổi mỗi điểm thưởng (VND)" }
        );
        await dbContext.SaveChangesAsync();

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

        await dbContext.Database.ExecuteSqlRawAsync(await File.ReadAllTextAsync("sample data/address.sql"));
        await dbContext.Database.ExecuteSqlRawAsync(await File.ReadAllTextAsync("sample data/coupon.sql"));
        await dbContext.Database.ExecuteSqlRawAsync(await File.ReadAllTextAsync("sample data/product.sql"));

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
        command.CommandText = await File.ReadAllTextAsync("sample data/order.sql");
        await dbContext.Database.OpenConnectionAsync();
        await command.ExecuteNonQueryAsync();
    }
}