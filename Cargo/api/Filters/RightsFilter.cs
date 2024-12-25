using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Filters;

public class RightsFilter : Attribute, IAsyncActionFilter
{
    private readonly UserServices _userServices;
    private readonly string[] _roles;

    public RightsFilter(UserServices userServices, string[] roles)
    {
        _userServices = userServices;
        _roles = roles;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext actioncontext, ActionExecutionDelegate next)
    {

        if (actioncontext.HttpContext.Session.GetInt32("UserId") is null)
        {
            Console.WriteLine($"You are not logged in!");
            actioncontext.HttpContext.Response.StatusCode = 401;
            return;
        }
        int userId = (int)actioncontext.HttpContext.Session.GetInt32("UserId")!;
        User user = await _userServices.GetUser(userId);

        if (!_roles.Contains(user.Role))
        {
            Console.WriteLine($"You have no right to this controller");
            actioncontext.HttpContext.Response.StatusCode = 401;
            return;
        }

        await next.Invoke();
    }
}