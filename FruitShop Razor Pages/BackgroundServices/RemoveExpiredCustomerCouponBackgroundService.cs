using Repository;
using Service.Customer;

namespace FruitShop_Razor_Pages.BackgroundServices;

public class RemoveExpiredCustomerCouponBackgroundService(
    ILogger<RemoveExpiredCustomerCouponBackgroundService> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(BusinessRuleConstants.Coupon
            .RemoveExpiredCustomerCouponBackgroundServiceDelayHours));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                logger.LogInformation("Start removing expired customer coupons");
                using var scope = serviceProvider.CreateScope();
                var couponService = scope.ServiceProvider.GetRequiredService<CouponService>();
                var numberOfCouponsRemoved = await couponService.RemoveAllExpiredCustomerCouponsAsync();
                logger.LogInformation(
                    "Finished removing expired customer coupons. Total coupons removed: {NumberOfCouponsRemoved}",
                    numberOfCouponsRemoved);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to remove expired customer coupons");
            }
        }
    }
}