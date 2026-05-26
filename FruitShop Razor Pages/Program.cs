using ElmahCore.Mvc;
using FruitShop_Razor_Pages.BackgroundService;
using FruitShop_Razor_Pages.Extensions;
using Markdig;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Repository;
using Repository.Data;
using Service;
using Service.Admin;
using Service.Customer;
using Service.SalesStaff;
using Service.DTOs.Admin.HealthCheck;
using OrderService = Service.Customer.OrderService;

var builder = WebApplication.CreateBuilder(args);

// Add secret configuration from appsettings.secret.json
builder.Configuration.AddJsonFile("appsettings.secret.json", false);

// Add Markdig Markdown pipeline for custom image rendering
builder.Services.AddKeyedSingleton("CustomImage", new MarkdownPipelineBuilder()
    .UseAdvancedExtensions()
    .Use(new CustomImageMarkdigExtensions())
    .Build());

// Add services
builder.AddRepositoryLevelServices();

builder.AddServiceLevelServices();

AddApplicationServices();

AddHostedService();

// Add Razor Pages services
builder.Services.AddRazorPages();

var app = builder.Build();

// Seed data
await app.SeedDataAsync();

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

void AddApplicationServices()
{
    builder.Services.AddScoped<AddressService>();
    builder.Services.AddScoped<ShippingAddressService>();
    builder.Services.AddScoped<CartService>();
    builder.Services.AddScoped<OrderService>();
    builder.Services.AddScoped<Service.SalesStaff.OrderService>();
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