using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.CustomerSupport;
using Service.DTOs.CustomerSupport;

namespace FruitShop_Razor_Pages.Pages.CustomerSupport
{
    [Authorize(Roles = Role.CustomerSupport)]
    public class ClassifyCommentModel(SupportService supportService) : PageModel
    {
        [BindProperty]
        public CommentDetailDto Comment { get; set; } = new CommentDetailDto();
        public async Task<IActionResult> OnGetAsync(long orderId, int productId)
        {
            var detail = await supportService.GetCommentDetaillAsync(orderId, productId);
            if (detail == null)
            {
                return NotFound("Không tìm thấy dữ liệu phản hồi");
            }
            Comment = detail;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await supportService.UpdateCommentClassificationAsync(
                Comment.OrderId, Comment.ProductId, Comment.CommentClassification, Comment.ResolutionMessage);
            TempData["SuccessMessage"] = "Cập nhật trạng thái thành công!";
            return RedirectToPage("CustomerSupport");
        }
    }
}
