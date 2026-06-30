using Microsoft.AspNetCore.Mvc;
using Service;

namespace FruitShop_Razor_Pages.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(BusinessRuleService businessRuleService) : ControllerBase
{
    [HttpGet("calculate-loyalty-points/{totalAmount:long}")]
    public long CalculateLoyaltyPoints(long totalAmount) =>
        businessRuleService.CalculateLoyaltyPoints(totalAmount);
}
