using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.DTOs.SalesStaff.Order;
using Service.SalesStaff;

namespace FruitShop_Razor_Pages.Pages.SalesStaff.Order;

[Authorize(Roles = Role.SalesStaff)]
public class Detail(OrderService service) : PageModel
{
    public OrderDetailDto? Order { get; set; }

    public async Task<IActionResult> OnGetAsync(long id)
    {
        try
        {
            Order = await service.GetOrderDetailByIdAsync(id);
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.Message;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostConfirmAsync(long id)
    {
        try
        {
            await service.ConfirmCodOrderAsync(id);
            TempData["SuccessMessage"] = "Đơn hàng đã được duyệt thành công.";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostCancelAsync(long id)
    {
        try
        {
            await service.CancelCodOrderAsync(id);
            TempData["SuccessMessage"] = "Đơn hàng đã bị huỷ.";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostMarkAsShippingAsync(long id)
    {
        try
        {
            await service.MarkOrderAsShippingAsync(id);
            TempData["SuccessMessage"] = "Đơn hàng đã được chuyển sang trạng thái giao hàng.";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToPage(new { id });
    }
}
