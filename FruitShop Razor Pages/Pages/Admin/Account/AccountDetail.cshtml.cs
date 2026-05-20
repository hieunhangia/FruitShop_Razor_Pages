using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.Constants;
using Service.Admin;
using Service.DTOs.Admin.Account;

namespace FruitShop_Razor_Pages.Pages.Admin.Account;

[Authorize(Roles = Role.Admin)]
public class AccountDetailModel(AccountService accountService) : PageModel
{
    public AccountDto? Account { get; set; }

    [BindProperty] public DateTime LockoutEnd { get; set; }

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

    public async Task<IActionResult> OnGetAsync(int userId)
    {
        try
        {
            Account = await accountService.GetAccountDetailAsync(userId);
            if ((Account.Roles?.Contains(Role.Shipper) ?? false) && !Account.Roles.Contains(Role.Admin) &&
                !Account.Roles.Contains(Role.Manager))
            {
                var shipperData = await accountService.GetShipperDataAsync(userId);
                ShipperDataInput = new ShipperDataInputModel
                {
                    Name = shipperData.Name,
                    PhoneNumber = shipperData.PhoneNumber
                };
            }

            LockoutEnd = Account.LockoutEnd?.LocalDateTime ?? DateTime.Now;
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostLockoutAsync(int userId)
    {
        try
        {
            await accountService.UpdateLockoutEndAsync(userId, LockoutEnd.ToUniversalTime());
            TempData["SuccessMessage"] = "Khóa tài khoản thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { userId });
    }

    public async Task<IActionResult> OnPostUnlockAsync(int userId)
    {
        try
        {
            await accountService.UpdateLockoutEndAsync(userId, null);
            TempData["SuccessMessage"] = "Mở khóa tài khoản thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { userId });
    }

    public async Task<IActionResult> OnPostUpdateLockoutEndAsync(int userId)
    {
        try
        {
            await accountService.UpdateLockoutEndAsync(userId, LockoutEnd.ToUniversalTime());
            TempData["SuccessMessage"] = "Cập nhật hạn khóa tài khoản thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { userId });
    }

    public async Task<IActionResult> OnPostRemoveSalesStaffRoleAsync(int userId)
    {
        try
        {
            await accountService.RemoveSalesStaffRoleAsync(userId);
            TempData["SuccessMessage"] = "Xóa vai trò thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { userId });
    }

    public async Task<IActionResult> OnPostAddSalesStaffRoleAsync(int userId)
    {
        try
        {
            await accountService.AddSalesStaffRoleAsync(userId);
            TempData["SuccessMessage"] = "Thêm vai trò thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { userId });
    }

    public async Task<IActionResult> OnPostRemoveShipperRoleAsync(int userId)
    {
        try
        {
            await accountService.RemoveShipperRoleAsync(userId);
            TempData["SuccessMessage"] = "Xóa vai trò thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { userId });
    }

    public async Task<IActionResult> OnPostAddShipperRoleAsync(int userId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await accountService.AddShipperRoleAsync(userId, new ShipperDataDto
            {
                Name = ShipperDataInput.Name,
                PhoneNumber = ShipperDataInput.PhoneNumber
            });
            TempData["SuccessMessage"] = "Thêm vai trò thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { userId });
    }

    public async Task<IActionResult> OnPostRemoveCustomerSupportRoleAsync(int userId)
    {
        try
        {
            await accountService.RemoveCustomerSupportRoleAsync(userId);
            TempData["SuccessMessage"] = "Xóa vai trò thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { userId });
    }

    public async Task<IActionResult> OnPostAddCustomerSupportRoleAsync(int userId)
    {
        try
        {
            await accountService.AddCustomerSupportRoleAsync(userId);
            TempData["SuccessMessage"] = "Thêm vai trò thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { userId });
    }

    public async Task<IActionResult> OnPostUpdateShipperDataAsync(int userId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await accountService.UpdateShipperDataAsync(userId, new ShipperDataDto
            {
                Name = ShipperDataInput.Name,
                PhoneNumber = ShipperDataInput.PhoneNumber
            });
            TempData["SuccessMessage"] = "Cập nhật dữ liệu shipper thành công";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { userId });
    }
}