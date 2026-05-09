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
public class UpdateModel(ShippingAddressService shippingAddressService, UserManager<User> userManager) : PageModel
{
    public int Id { get; set; }

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

    public bool IsDefault { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var customerId = int.Parse(userManager.GetUserId(User)!);
        var shippingAddress = await shippingAddressService.GetShippingAddressByIdAndCustomerIdAsync(customerId, id);
        if (shippingAddress == null)
        {
            ViewData["ErrorMessage"] = "Yêu cầu không hợp lệ hoặc địa chỉ giao hàng không tồn tại.";
            return Page();
        }

        Id = id;
        Input.RecipientName = shippingAddress.RecipientName;
        Input.RecipientPhoneNumber = shippingAddress.RecipientPhoneNumber;
        Input.ProvinceCode = shippingAddress.ProvinceCode;
        Input.CommuneCode = shippingAddress.CommuneCode;
        Input.SpecificAddress = shippingAddress.SpecificAddress;
        IsDefault = shippingAddress.IsDefault;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var customerId = int.Parse(userManager.GetUserId(User)!);
        try
        {
            await shippingAddressService.UpdateShippingAddressAsync(customerId, new UpdateShippingAddressDto
            {
                Id = id,
                RecipientName = Input.RecipientName,
                RecipientPhoneNumber = Input.RecipientPhoneNumber,
                CommuneCode = Input.CommuneCode,
                SpecificAddress = Input.SpecificAddress
            });
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }

        TempData["SuccessMessage"] = "Địa chỉ giao hàng đã được cập nhật thành công.";
        return RedirectToPage("Manage");
    }

    public async Task<IActionResult> OnPostSetDefaultAsync(int id)
    {
        var customerId = int.Parse(userManager.GetUserId(User)!);
        try
        {
            await shippingAddressService.SetDefaultShippingAddressAsync(customerId, id);
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }

        TempData["SuccessMessage"] = "Địa chỉ giao hàng đã được đặt làm mặc định thành công.";
        return RedirectToPage("Manage");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var customerId = int.Parse(userManager.GetUserId(User)!);
        try
        {
            await shippingAddressService.DeleteShippingAddressAsync(customerId, id);
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return Page();
        }

        TempData["SuccessMessage"] = "Địa chỉ giao hàng đã được xóa thành công.";
        return RedirectToPage("Manage");
    }
}