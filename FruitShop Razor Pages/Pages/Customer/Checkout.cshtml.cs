using System.ComponentModel.DataAnnotations;
using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.Customer;
using Service.DTOs.Customer.Cart;
using Service.DTOs.Customer.Coupon;
using Service.DTOs.Customer.Order;
using Service.DTOs.Customer.ShippingAddress;

namespace FruitShop_Razor_Pages.Pages.Customer;

[Authorize(Roles = Role.Customer)]
public class CheckoutModel(
    OrderService orderService,
    ShippingAddressService shippingAddressService,
    CartService cartService,
    CouponService couponService) : PageModel
{
    public List<CartItemDto> Cart { get; set; } = [];

    public List<ShippingAddressDto> ShippingAddresses { get; set; } = [];

    public List<CouponInCheckoutPageDto> AvailableCoupons { get; set; } = [];

    [Required(ErrorMessage = "Vui lòng chọn địa chỉ giao hàng.")]
    [BindProperty]
    public int SelectedAddressId { get; set; }

    [BindProperty] public int? SelectedCustomerCouponId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
    [BindProperty]
    public PaymentMethod PaymentMethod { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var customerId = User.GetUserId();
        Cart = await cartService.GetSelectedCartItemsAsync(customerId);

        if (Cart.Count == 0)
        {
            return RedirectToPage("Cart");
        }

        var numberOfUpdateItems =
            await cartService.SyncCartWithInventoryAsync(customerId, Cart.Select(ci => ci.ProductId).ToList());
        if (numberOfUpdateItems > 0)
        {
            TempData["ErrorMessage"] =
                "Một số sản phẩm được chọn thanh toán đã được cập nhật do thay đổi về tình trạng tồn kho. Vui lòng kiểm tra lại giỏ hàng của bạn.";
            return RedirectToPage("Cart");
        }

        ShippingAddresses = await shippingAddressService.GetShippingAddressesByCustomerIdAsync(customerId);
        AvailableCoupons = await couponService.GetAvailableCouponsForOrderAsync(customerId,
            Cart.Sum(ci => ci.ProductPrice * ci.Quantity));
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
            return PaymentMethod switch
            {
                PaymentMethod.CashOnDelivery => await ProcessCashOnDeliveryAsync(),
                PaymentMethod.QRCode => await ProcessQRCodePaymentAsync(),
                _ => RedirectToPage("/Everyone/Index")
            };

        TempData["ErrorMessage"] = "Đã có lỗi trong quá trình xử lý đơn hàng.";
        return RedirectToPage("/Everyone/Index");
    }

    private async Task<IActionResult> ProcessCashOnDeliveryAsync()
    {
        var customerId = User.GetUserId();
        var customerEmail = User.GetUserEmail();
        try
        {
            await orderService.CreateCashOnDeliveryOrderAsync(customerId, new CreateCashOnDeliveryOrderDto
            {
                CustomerEmail = customerEmail,
                ShippingAddressId = SelectedAddressId,
                CustomerCouponId = SelectedCustomerCouponId
            });
            return RedirectToPage("/Customer/Order/OrderSuccess");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return RedirectToPage("/Everyone/Index");
        }
    }

    private async Task<IActionResult> ProcessQRCodePaymentAsync()
    {
        var customerId = User.GetUserId();
        var customerEmail = User.GetUserEmail();
        try
        {
            var checkoutUrl = await orderService.CreateQRCodePaymentOrderAsync(customerId, new CreateQrCodePaymentDto
            {
                CustomerEmail = customerEmail,
                ShippingAddressId = SelectedAddressId,
                CustomerCouponId = SelectedCustomerCouponId,
                ReturnUrl = Url.Page("/Customer/Order/OrderSuccess", null, null, Request.Scheme)!,
                CancelUrl = Url.Page("/Customer/Order/QrCodePaymentOrderCancelled", null, null, Request.Scheme)!
            });
            return Redirect(checkoutUrl);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return RedirectToPage("/Everyone/Index");
        }
    }
}