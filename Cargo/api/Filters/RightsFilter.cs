using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Filters;

public class RightsFilter : Attribute, IAsyncActionFilter
{
    private readonly string[] _roles;
    public static string[] validRoles = ["admin", "warehouse manager", "inventory Manager", "floor Manager", "operative", "supervisor", "analyst", "logistics", "sales"];

    public RightsFilter(string[] roles)
    {
        _roles = roles.Select(r => r.ToLowerInvariant()).ToArray();
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext actioncontext, ActionExecutionDelegate next)
    {

        if (actioncontext.HttpContext.Session.GetInt32("UserId") is null)
        {
            Console.WriteLine($"You are not logged in!");
            actioncontext.HttpContext.Response.StatusCode = 401;
            return;
        }
        if (!_roles.Contains(actioncontext.HttpContext.Session.GetString("Role")))
        {
            Console.WriteLine($"You have no right to this controller");
            actioncontext.HttpContext.Response.StatusCode = 401;
            return;
        }

        await next.Invoke();
    }
}