using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.CustomerSupport;
using Service.DTOs;
using Service.DTOs.CustomerSupport;

namespace FruitShop_Razor_Pages.Pages.CustomerSupport
{
    [Authorize(Roles = "CustomerSupport")]
    public class CustomerSupportModel(SupportService supportService) : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public PagedAndSortedRequest<CommentFilterDto> PagedAndSortedRequest { get; set; } = new();

        public PagedAndSortedDto<CommentSummaryDto>? PagedAndSortedResult { get; set; }
        public async Task<IActionResult> OnGetAsync(bool? isSearch)
        {
            if (PagedAndSortedRequest.Filter is { StartDate: not null, EndDate: not null })
            {
                if (PagedAndSortedRequest.Filter.StartDate > PagedAndSortedRequest.Filter.EndDate)
                {
                    ModelState.AddModelError(string.Empty, "Ngày bắt đầu lọc phải nhỏ hơn hoặc bằng ngày kết thúc.");
                }
            }

            if (!ModelState.IsValid)
            {
                PagedAndSortedResult = new PagedAndSortedDto<CommentSummaryDto>([], 0, 0, 5, "", SortDirection.Ascending);
                return Page();
            }

            if (isSearch == true)
            {
                PagedAndSortedRequest.PageIndex = 1;
            }

            var supportId = User.GetUserId();

            PagedAndSortedResult = await supportService.GetAssignedCommentListAsync(supportId, PagedAndSortedRequest);
            return Page();
        }
    }
}
