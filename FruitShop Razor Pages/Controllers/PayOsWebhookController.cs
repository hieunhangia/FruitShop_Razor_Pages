using Microsoft.AspNetCore.Mvc;
using PayOS;
using PayOS.Models.Webhooks;
using Service.Customer;

namespace FruitShop_Razor_Pages.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayOsWebhookController(PayOSClient client, OrderService orderService) : ControllerBase
{
    [HttpPost("payment")]
    public async Task<ActionResult> VerifyPayment(Webhook webhook)
    {
        try
        {
            var webhookData = await client.Webhooks.VerifyAsync(webhook);
            await orderService.ConfirmQrCodePaymentOrderAsync(webhookData.OrderCode);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}