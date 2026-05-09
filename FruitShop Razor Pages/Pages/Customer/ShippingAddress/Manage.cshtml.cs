using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Repository.Models.Users;
using Service.Customer;
using Service.DTOs.Customer.ShippingAddress;

namespace FruitShop_Razor_Pages.Pages.Customer.ShippingAddress;

[Authorize(Roles = Role.Customer)]
public class ManageModel(ShippingAddressService shippingAddressService, UserManager<User> userManager) : PageModel
{
    public List<ShippingAddressDto> ShippingAddresses { get; set; } = [];

    public async Task OnGetAsync()
    {
        var customerId = int.Parse(userManager.GetUserId(User)!);
        ShippingAddresses = await shippingAddressService.GetShippingAddressesByCustomerIdAsync(customerId);
    }
}