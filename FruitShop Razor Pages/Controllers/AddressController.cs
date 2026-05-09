using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace FruitShop_Razor_Pages.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressController(AddressService addressService) : ControllerBase
{
    [HttpGet("provinces")]
    public async Task<IActionResult> GetProvinces() => Ok(await addressService.GetProvincesAsync());

    [HttpGet("provinces/{provinceCode}")]
    public async Task<IActionResult> GetProvinceByCode(string provinceCode) =>
        Ok(await addressService.GetProvinceByCodeAsync(provinceCode));

    [HttpGet("provinces/{provinceCode}/communes")]
    public async Task<IActionResult> GetCommunesByProvinceCode(string provinceCode) =>
        Ok(await addressService.GetCommunesByProvinceCodeAsync(provinceCode));

    [HttpGet("communes/{communeCode}")]
    public async Task<IActionResult> GetCommuneByCode(string communeCode) =>
        Ok(await addressService.GetCommuneByCodeAsync(communeCode));
}