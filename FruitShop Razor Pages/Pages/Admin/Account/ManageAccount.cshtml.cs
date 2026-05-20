using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Constants;
using Service.Admin;
using Service.DTOs;
using Service.DTOs.Admin.Account;

namespace FruitShop_Razor_Pages.Pages.Admin.Account;

[Authorize(Roles = Role.Admin)]
public class ManageAccountModel(AccountService accountService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public PagedAndSortedRequest<AccountFilter> PagedAndSortedRequest { get; set; } = new();

    public PagedAndSortedDto<AccountDto>? PagedAndSortedResult { get; set; }

    public async Task<IActionResult> OnGetAsync(bool? isSearch)
    {
        if (!ModelState.IsValid)
        {
            PagedAndSortedResult = new PagedAndSortedDto<AccountDto>([], 0, 0, 0,
                "", SortDirection.Ascending);
            return Page();
        }

        if (isSearch == true)
        {
            PagedAndSortedRequest.PageIndex = 1;
        }

        PagedAndSortedResult = await accountService.GetAccountsAsync(PagedAndSortedRequest);

        return Page();
    }
}