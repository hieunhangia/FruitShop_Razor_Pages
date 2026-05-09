using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Models.Address;
using Repository.Models.Orders;
using Repository.Models.Products;
using Repository.Models.Users;

namespace Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<Commune> Communes { get; set; }
    public DbSet<Province> Provinces { get; set; }

    public DbSet<ShippingAddress> ShippingAddresses { get; set; }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductUnit> ProductUnits { get; set; }

    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureIdentity(modelBuilder);
        ConfigureUser(modelBuilder.Entity<User>());
        ConfigureCommune(modelBuilder.Entity<Commune>());
        ConfigureShippingAddress(modelBuilder.Entity<ShippingAddress>());
        ConfigureProduct(modelBuilder.Entity<Product>());
        ConfigureCartItems(modelBuilder.Entity<CartItem>());
        ConfigureOrder(modelBuilder.Entity<Order>());
        ConfigureOrderItems(modelBuilder.Entity<OrderItem>());
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

    private static void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        builder.HasMany<CartItem>()
            .WithOne(ci => ci.Customer)
            .HasForeignKey(ci => ci.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureCommune(EntityTypeBuilder<Commune> builder)
    {
        builder.HasOne(c => c.Province)
            .WithMany(p => p.Communes)
            .HasForeignKey(c => c.ProvinceCode)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureShippingAddress(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.Property(sa => sa.RecipientPhoneNumber)
            .HasMaxLength(BusinessRuleConstants.Model.ShippingAddress.RecipientPhoneNumberLength)
            .IsFixedLength();
    }

    private static void ConfigureProduct(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne(p => p.ProductUnit)
            .WithMany()
            .HasForeignKey(p => p.ProductUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Categories)
            .WithMany(c => c.Products)
            .UsingEntity(j => j.ToTable("ProductCategories"));

        builder.HasMany<CartItem>()
            .WithOne(ci => ci.Product)
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<OrderItem>()
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureCartItems(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => new { ci.CustomerId, ci.ProductId });
    }

    private static void ConfigureOrder(EntityTypeBuilder<Order> builder)
    {
        builder.HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Shipper)
            .WithMany()
            .HasForeignKey(o => o.ShipperId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.OrderShippings)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.QrCodePaymentData)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureOrderItems(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => new { oi.OrderId, oi.ProductId });
    }
}