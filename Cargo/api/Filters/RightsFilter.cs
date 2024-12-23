using Microsoft.AspNetCore.Mvc.Filters;

public class RightsFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext actioncontext, ActionExecutionDelegate next)
    {
        // Example: Get a specific query parameter by key (e.g., "id")
        if (false)
        {
            Console.WriteLine($"example");
            actioncontext.HttpContext.Response.StatusCode = 401;
            return;
        }
        await next.Invoke();
    }
}