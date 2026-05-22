using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.Customer;
using Service.DTOs.Customer.Order;

namespace FruitShop_Razor_Pages.Pages.Customer.Order;

[Authorize(Roles = Role.Customer)]
public class CreateProductReviewModel(ProductReviewService reviewService) : PageModel
{
    [BindProperty]
    public CreateProductReviewDto Input { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public async Task<IActionResult> OnGetAsync(int orderId, int productId)
    {
        var customerId = User.GetUserId();
        bool canReview = await reviewService.CanReviewAsync(orderId, productId, customerId);

        if (!canReview)
        {
            return RedirectToPage("/Customer/Order/OrderDetail", new { id = orderId });
        }

        Input.OrderId = orderId;
        Input.ProductId = productId;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await reviewService.CreateReviewAsync(Input);
            return RedirectToPage("/Customer/Order/OrderDetail", new { id = Input.OrderId });
        }
        catch (Exception ex) {
            var detailedError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

            // In ra màn hình để nhìn thấy tận mắt Database đang chửi lỗi gì
            ErrorMessage = $"Lỗi Database: {detailedError}";
            return Page();
        }
    }
}