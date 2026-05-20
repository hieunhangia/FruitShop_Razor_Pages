using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.Models.Address;
using Repository.Models.Coupons;
using Repository.Models.Orders;
using Repository.Models.Products;
using Repository.Models.Users;

namespace Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<Commune> Communes { get; set; }
    public DbSet<Province> Provinces { get; set; }

    public DbSet<CustomerData> CustomerData { get; set; }
    public DbSet<ShippingAddress> ShippingAddresses { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<CustomerCoupon> CustomerCoupons { get; set; }
    public DbSet<ShipperData> ShipperData { get; set; }
    public DbSet<CustomerSupportData> CustomerSupportData { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductUnit> ProductUnits { get; set; }

    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
            v => v.ToLocalTime());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }
        }

        ConfigureIdentity(modelBuilder);
        ConfigureCommune(modelBuilder.Entity<Commune>());
        ConfigureCustomerData(modelBuilder.Entity<CustomerData>());
        ConfigureShippingAddress(modelBuilder.Entity<ShippingAddress>());
        ConfigureCustomerCoupon(modelBuilder.Entity<CustomerCoupon>());
        ConfigureShipperData(modelBuilder.Entity<ShipperData>());
        ConfigureCustomerSupportData(modelBuilder.Entity<CustomerSupportData>());
        ConfigureProduct(modelBuilder.Entity<Product>());
        ConfigureCartItems(modelBuilder.Entity<CartItem>());
        ConfigureOrder(modelBuilder.Entity<Order>());
        ConfigureOrderItems(modelBuilder.Entity<OrderItem>());
        ConfigureProductReview(modelBuilder.Entity<ProductReview>());
    }

    private static void ConfigureIdentity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
    }

    private static void ConfigureCommune(EntityTypeBuilder<Commune> entity)
    {
        entity.HasOne(c => c.Province)
            .WithMany(p => p.Communes)
            .HasForeignKey(c => c.ProvinceCode)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureCustomerData(EntityTypeBuilder<CustomerData> entity)
    {
        entity.HasKey(cd => cd.CustomerId);

        entity.HasOne(cd => cd.Customer)
            .WithOne(u => u.CustomerData)
            .HasForeignKey<CustomerData>(cd => cd.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureShippingAddress(EntityTypeBuilder<ShippingAddress> entity)
    {
        entity.HasOne(sa => sa.CustomerData)
            .WithMany(cd => cd.ShippingAddresses)
            .HasForeignKey(sa => sa.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(sa => sa.Commune)
            .WithMany()
            .HasForeignKey(sa => sa.CommuneCode)
            .OnDelete(DeleteBehavior.Restrict);

        entity.Property(sa => sa.RecipientPhoneNumber)
            .HasMaxLength(BusinessRuleConstants.Model.ShippingAddress.RecipientPhoneNumberLength)
            .IsFixedLength();
    }

    private static void ConfigureCustomerCoupon(EntityTypeBuilder<CustomerCoupon> entity)
    {
        entity.HasOne(cc => cc.Customer)
            .WithMany(cd => cd.CustomerCoupons)
            .HasForeignKey(cc => cc.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(cc => cc.Coupon)
            .WithMany()
            .HasForeignKey(cc => cc.CouponId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureShipperData(EntityTypeBuilder<ShipperData> entity)
    {
        entity.HasKey(sd => sd.ShipperId);

        entity.HasOne(sd => sd.Shipper)
            .WithOne(u => u.ShipperData)
            .HasForeignKey<ShipperData>(sd => sd.ShipperId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureCustomerSupportData(EntityTypeBuilder<CustomerSupportData> entity)
    {
        entity.HasKey(csd => csd.CustomerSupportId);

        entity.HasOne(csd => csd.CustomerSupport)
            .WithOne(u => u.CustomerSupportData)
            .HasForeignKey<CustomerSupportData>(csd => csd.CustomerSupportId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureProduct(EntityTypeBuilder<Product> entity)
    {
        entity.HasOne(p => p.ProductUnit)
            .WithMany()
            .HasForeignKey(p => p.ProductUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(p => p.Categories)
            .WithMany(c => c.Products)
            .UsingEntity(j => j.ToTable("ProductCategories"));

        entity.HasMany<CartItem>()
            .WithOne(ci => ci.Product)
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany<OrderItem>()
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureCartItems(EntityTypeBuilder<CartItem> entity)
    {
        entity.HasKey(ci => new { ci.CustomerId, ci.ProductId });

        entity.HasOne(ci => ci.Customer)
            .WithMany(cd => cd.CartItems)
            .HasForeignKey(ci => ci.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureOrder(EntityTypeBuilder<Order> entity)
    {
        entity.OwnsOne(p => p.ShippingAddressSnapshot, builderAction => { builderAction.ToJson(); });

        entity.HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(o => o.Shipper)
            .WithMany()
            .HasForeignKey(o => o.ShipperId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(o => o.OrderShippings)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(o => o.QrCodePaymentData)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureOrderItems(EntityTypeBuilder<OrderItem> entity)
    {
        entity.HasKey(oi => new { oi.OrderId, oi.ProductId });

        entity.OwnsOne(oi => oi.ProductSnapshot, builderAction => { builderAction.ToJson(); });
    }

    private static void ConfigureProductReview(EntityTypeBuilder<ProductReview> entity)
    {
        entity.HasKey(pr => new { pr.OrderId, pr.ProductId });

        entity.HasOne(pr => pr.OrderItem)
            .WithOne(oi => oi.ProductReview)
            .HasForeignKey<ProductReview>(pr => new { pr.OrderId, pr.ProductId })
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(pr => pr.Product)
            .WithMany(p => p.ProductReviews)
            .HasForeignKey(pr => pr.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(pr => pr.AssignedCustomerSupport)
            .WithMany(cs => cs.ProductReviews)
            .HasForeignKey(pr => pr.AssignedCustomerSupportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}