using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository;
using Repository.Constants;
using Service.Admin;
using Service.DTOs.Admin.Account;

namespace FruitShop_Razor_Pages.Pages.Admin.Account;

[Authorize(Roles = Role.Admin)]
public class CreateStaffAccountModel(AccountService accountService) : PageModel
{
    public List<SelectListItem> StaffRoles { get; set; } = [];

    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(BusinessRuleConstants.Identity.Password.RequiredLength,
            ErrorMessage = "Mật khẩu phải có ít nhất {1} ký tự.")]
        [MaxLength(BusinessRuleConstants.Identity.Password.MaxLength,
            ErrorMessage = "Mật khẩu không được vượt quá {1} ký tự.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn ít nhất một vai trò.")]
        public List<string> SelectedRoleNames { get; set; } = [];
    }

    [BindProperty] public ShipperDataInputModel ShipperDataInput { get; set; } = new();

    public class ShipperDataInputModel
    {
        [Required(ErrorMessage = "Tên Người giao hàng là bắt buộc.")]
        [MaxLength(BusinessRuleConstants.Model.ShipperData.NameMaxLength,
            ErrorMessage = "Tên Người giao hàng không được vượt quá {1} kí tự.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại Người giao hàng là bắt buộc.")]
        [RegularExpression(BusinessRuleConstants.Model.ShipperData.PhoneNumberPattern,
            ErrorMessage = "Số điện thoại Người giao hàng không hợp lệ.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public void OnGet()
    {
        StaffRoles = Role.StaffRoles.Select(role => new SelectListItem
        {
            Value = role,
            Text = role
        }).ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            StaffRoles = Role.StaffRoles.Select(role => new SelectListItem
            {
                Value = role,
                Text = role
            }).ToList();
            return Page();
        }

        try
        {
            var createStaffAccountDto = new CreateStaffAccountDto
            {
                Email = Input.Email,
                Password = Input.Password,
                SelectedRoleNames = Input.SelectedRoleNames
            };

            if (Input.SelectedRoleNames.Contains(Role.Shipper))
            {
                createStaffAccountDto.ShipperData = new ShipperDataDto
                {
                    Name = ShipperDataInput.Name,
                    PhoneNumber = ShipperDataInput.PhoneNumber
                };
            }

            await accountService.CreateStaffAccountAsync(createStaffAccountDto);
            TempData["SuccessMessage"] = "Tài khoản nhân viên đã được tạo thành công.";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage("ManageAccount");
    }
}