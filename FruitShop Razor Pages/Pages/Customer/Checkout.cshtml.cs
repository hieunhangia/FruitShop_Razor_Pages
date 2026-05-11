using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Repository.Models.Users;
using Service.Customer;
using Service.DTOs.Customer.Order;
using Service.DTOs.Customer.ShippingAddress;

namespace FruitShop_Razor_Pages.Pages.Customer;

[Authorize(Roles = Role.Customer)]
public class CheckoutModel(
    OrderService orderService,
    ShippingAddressService shippingAddressService,
    CartService cartService,
    UserManager<User> userManager) : PageModel
{
    public class ProductItem
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ProductUnit { get; set; } = string.Empty;
        public long Price { get; set; }
        public int Quantity { get; set; }
    }

    public List<ProductItem> ProductItems { get; set; } = [];

    public List<ShippingAddressDto> ShippingAddresses { get; set; } = [];

    [Required(ErrorMessage = "Vui lòng chọn địa chỉ giao hàng.")]
    [BindProperty]
    public int SelectedAddressId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
    [BindProperty]
    public PaymentMethod PaymentMethod { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var customerId = int.Parse(userManager.GetUserId(User)!);
        var cart = await cartService.GetSelectedCartItemsAsync(customerId);
        if (cart.HasUpdates)
        {
            TempData["ErrorMessage"] =
                "Một số sản phẩm được chọn thanh toán đã được cập nhật do thay đổi về tình trạng tồn kho. Vui lòng kiểm tra lại giỏ hàng của bạn.";
            return RedirectToPage("Cart");
        }

        if (cart.CartItems.Count == 0)
        {
            TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một sản phẩm để thanh toán.";
            return RedirectToPage("Cart");
        }

        ProductItems = cart.CartItems.Select(ci => new ProductItem
        {
            Name = ci.ProductName,
            ImageUrl = ci.ProductImageUrl,
            ProductUnit = ci.ProductUnit,
            Price = ci.ProductPrice,
            Quantity = ci.Quantity
        }).ToList();
        ShippingAddresses = await shippingAddressService.GetShippingAddressesByCustomerIdAsync(customerId);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
            return PaymentMethod switch
            {
                PaymentMethod.CashOnDelivery => await ProcessCashOnDeliveryAsync(),
                PaymentMethod.QRCode => await ProcessQRCodePaymentAsync(),
                _ => RedirectToPage("/Index")
            };

        TempData["ErrorMessage"] = "Đã có lỗi trong quá trình xử lý đơn hàng. Vui lòng thử lại.";
        return RedirectToPage("/Index");
    }

    private async Task<IActionResult> ProcessCashOnDeliveryAsync()
    {
        var customerId = int.Parse(userManager.GetUserId(User)!);
        try
        {
            await orderService.CreateCashOnDeliveryOrderAsync(customerId, new CreateOrderDto
            {
                OrderDate = DateTime.Now,
                ShippingAddressId = SelectedAddressId
            });
            return RedirectToPage("/Customer/Order/OrderSuccess");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return RedirectToPage("/Index");
        }
    }

    private async Task<IActionResult> ProcessQRCodePaymentAsync()
    {
        throw new NotImplementedException();
    }
}