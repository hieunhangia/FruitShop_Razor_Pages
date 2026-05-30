using Repository;
using Service.Customer;

namespace FruitShop_Razor_Pages.BackgroundServices;

public class CancelExpiredQrCodePaymentOrderBackgroundService(
    ILogger<CancelExpiredQrCodePaymentOrderBackgroundService> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(BusinessRuleConstants.Order
            .CancelExpiredQrCodePaymentOrderBackgroundServiceDelayMinutes));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                logger.LogInformation("Start canceling expired QR code payment orders");
                using var scope = serviceProvider.CreateScope();
                var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
                var numberOfOrdersCancelled = await orderService.CancelAllExpiredQrCodePaymentOrdersAsync();
                logger.LogInformation(
                    "Finished canceling expired QR code payment orders. Total orders cancelled: {NumberOfOrdersCancelled}",
                    numberOfOrdersCancelled);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to cancel expired QR code payment orders");
            }
        }
    }
}