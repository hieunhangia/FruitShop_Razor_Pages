using Microsoft.AspNetCore.Mvc;
using Repository;

namespace FruitShop_Razor_Pages.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    [HttpGet("calculate-loyalty-points/{totalAmount:long}")]
    public long CalculateLoyaltyPoints(long totalAmount) =>
        BusinessRuleConstants.LoyaltyPoint.CalculateLoyaltyPoints(totalAmount);
}