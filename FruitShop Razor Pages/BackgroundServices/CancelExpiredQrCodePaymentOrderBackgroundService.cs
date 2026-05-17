using Repository;
using Service.Customer;

namespace FruitShop_Razor_Pages.BackgroundService;

public class CancelExpiredQrCodePaymentOrderBackgroundService(
    ILogger<CancelExpiredQrCodePaymentOrderBackgroundService> logger,
    IServiceProvider serviceProvider) : Microsoft.Extensions.Hosting.BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(BusinessRuleConstants.Order
            .CancelExpiredQrCodePaymentOrderBackgroundServiceDelayMinutes));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            logger.LogInformation("Start canceling expired QR code payment orders");
            using var scope = serviceProvider.CreateScope();
            var orderService = scope.ServiceProvider.GetRequiredService<OrderService>();
            var numberOfOrdersCancelled = await orderService.CancelAllExpiredQrCodePaymentOrdersAsync();
            logger.LogInformation(
                "Finished canceling expired QR code payment orders. Total orders cancelled: {NumberOfOrdersCancelled}",
                numberOfOrdersCancelled);
        }
    }
}