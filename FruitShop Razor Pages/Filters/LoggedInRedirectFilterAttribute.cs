using FruitShop_Razor_Pages.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FruitShop_Razor_Pages.Filters;

[AttributeUsage(AttributeTargets.Class)]
public class LoggedInRedirectFilterAttribute : Attribute, IAsyncPageFilter
{
    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
        PageHandlerExecutionDelegate next)
    {
        if (context.HttpContext.User.IsAuthenticated())
        {
            context.Result = new RedirectToPageResult("/Everyone/Index");
            return;
        }

        await next();
    }
}