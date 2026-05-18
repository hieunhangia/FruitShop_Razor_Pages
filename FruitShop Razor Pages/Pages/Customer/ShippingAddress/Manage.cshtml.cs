using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.Customer;
using Service.DTOs.Customer.ShippingAddress;

namespace FruitShop_Razor_Pages.Pages.Customer.ShippingAddress;

[Authorize(Roles = Role.Customer)]
public class ManageModel(ShippingAddressService shippingAddressService) : PageModel
{
    public List<ShippingAddressDto> ShippingAddresses { get; set; } = [];

    public async Task OnGetAsync()
    {
        var customerId = User.GetUserId();
        ShippingAddresses = await shippingAddressService.GetShippingAddressesByCustomerIdAsync(customerId);
    }
}