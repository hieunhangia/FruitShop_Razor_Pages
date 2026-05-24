using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.Customer;
using Service.DTOs.Customer.Cart;

namespace FruitShop_Razor_Pages.Pages.Customer;

[Authorize(Roles = Role.Customer)]
public class CartModel(CartService cartService) : PageModel
{
    public List<CartItemDto> Cart { get; set; } = [];

    public long TotalSelectedAmount { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var customerId = User.GetUserId();
        Cart = await cartService.GetCartAsync(customerId);

        if (Cart.Count == 0)
        {
            return Page();
        }

        var numberOfUpdateItems =
            await cartService.SyncCartWithInventoryAsync(customerId, Cart.Select(ci => ci.ProductId).ToList());
        if (numberOfUpdateItems > 0)
        {
            TempData["ErrorMessage"] =
                "Một số sản phẩm trong giỏ hàng đã được cập nhật do thay đổi về tình trạng tồn kho. Vui lòng kiểm tra lại giỏ hàng của bạn.";
            return RedirectToPage();
        }

        TotalSelectedAmount = Cart.Where(ci => ci.IsSelected).Sum(ci => ci.Quantity * ci.ProductPrice);

        return Page();
    }

    public async Task<IActionResult> OnPostSelectAllAsync(bool isSelected, int[] productIds)
    {
        var customerId = User.GetUserId();
        foreach (var productId in productIds)
        {
            await cartService.UpdateCartItemSelectionAsync(customerId, productId, isSelected);
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSelectAsync(int productId, bool isSelected)
    {
        var customerId = User.GetUserId();
        await cartService.UpdateCartItemSelectionAsync(customerId, productId, isSelected);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveAsync(int productId)
    {
        var customerId = User.GetUserId();
        await cartService.UpdateCartItemQuantityAsync(customerId, productId, 0);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int productId, string quantity)
    {
        var customerId = User.GetUserId();
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