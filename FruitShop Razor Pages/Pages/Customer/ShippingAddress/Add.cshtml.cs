using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.Constants;
using Repository.Models.Users;
using Service.Customer;
using Service.DTOs.Customer.ShippingAddress;

namespace FruitShop_Razor_Pages.Pages.Customer.ShippingAddress;

[Authorize(Roles = Role.Customer)]
public class AddModel(ShippingAddressService shippingAddressService, UserManager<User> userManager) : PageModel
{
    [BindProperty] public string? ReturnUrl { get; set; }

    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Tên người nhận là bắt buộc.")]
        [MaxLength(BusinessRuleConstants.Model.ShippingAddress.RecipientNameMaxLength,
            ErrorMessage = "Tên người nhận không được vượt quá {1} ký tự.")]
        public string RecipientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại người nhận là bắt buộc.")]
        [RegularExpression(BusinessRuleConstants.Model.ShippingAddress.RecipientPhoneNumberPattern,
            ErrorMessage = "Số điện thoại người nhận không hợp lệ.")]
        public string RecipientPhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tỉnh/thành phố là bắt buộc.")]
        public string ProvinceCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xã/phường là bắt buộc.")]
        public string CommuneCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ cụ thể là bắt buộc.")]
        [MaxLength(BusinessRuleConstants.Model.ShippingAddress.SpecificAddressMaxLength,
            ErrorMessage = "Địa chỉ cụ thể không được vượt quá {1} ký tự.")]
        public string SpecificAddress { get; set; } = string.Empty;
    }

    public void OnGet() => ReturnUrl = Request.GetTypedHeaders().Referer?.PathAndQuery;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var customerId = int.Parse(userManager.GetUserId(User)!);
        await shippingAddressService.AddShippingAddressAsync(customerId, new AddShippingAddressDto
        {
            RecipientName = Input.RecipientName,
            RecipientPhoneNumber = Input.RecipientPhoneNumber,
            CommuneCode = Input.CommuneCode,
            SpecificAddress = Input.SpecificAddress
        });

        TempData["SuccessMessage"] = "Địa chỉ giao hàng đã được thêm thành công.";
        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
        {
            return LocalRedirect(ReturnUrl);
        }

        return RedirectToPage("Manage");
    }
}