using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Repository.Models.Orders;
using Repository.Models.Users;
using Service.Customer;

namespace FruitShop_Razor_Pages.Pages.Customer;

[Authorize(Roles = Role.Customer)]
public class CartModel(CartService cartService, UserManager<User> userManager) : PageModel
{
    public List<CartItem> Cart { get; set; } = [];

    public bool HasUpdates { get; set; }

    public long TotalSelectedAmount { get; set; }

    public async Task OnGetAsync()
    {
        var customerId = int.Parse(userManager.GetUserId(User)!);
        (Cart, HasUpdates) = await cartService.GetCartAsync(customerId);
        TotalSelectedAmount = Cart.Where(ci => ci.IsSelected).Sum(ci => ci.Quantity * ci.Product!.Price);
    }

    public async Task<IActionResult> OnPostSelectAsync(int productId, bool isSelected)
    {
        var customerId = int.Parse(userManager.GetUserId(User)!);
        await cartService.UpdateCartItemSelectionAsync(customerId, productId, isSelected);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveAsync(int productId)
    {
        var customerId = int.Parse(userManager.GetUserId(User)!);
        await cartService.UpdateCartItemQuantityAsync(customerId, productId, 0);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int productId, string quantity)
    {
        var customerId = int.Parse(userManager.GetUserId(User)!);
        try
        {
            if (int.TryParse(quantity, out var quantityValue))
            {
                await cartService.UpdateCartItemQuantityAsync(customerId, productId, quantityValue);
            }
            else
            {
                TempData["ErrorMessage"] = "Số lượng không hợp lệ.";
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage();
    }
}