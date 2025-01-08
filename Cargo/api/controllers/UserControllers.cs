using Microsoft.AspNetCore.Mvc;


[Route("Cargohub")]
public class UserControllers : Controller
{
    private readonly UserServices _userServices;
    private readonly WarehouseServices _warehouseServices;

    public UserControllers(UserServices userServices, WarehouseServices warehouseServices)
    {
        _userServices = userServices;
        _warehouseServices = warehouseServices;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        try
        {
            User LoggedInUser = await _userServices.GetUser(user.Username!, user.Password!);
            if (LoggedInUser is null) return BadRequest("User not found.");
            if (HttpContext.Session.GetInt32("UserId") != null) return BadRequest("You are already logged in on this session!");
            if (HttpContext.Session.GetInt32("UserId") == LoggedInUser.Id) return BadRequest("User is already logged in. ");
            HttpContext.Session.SetInt32("UserId", LoggedInUser.Id);
            HttpContext.Session.SetString("Role", LoggedInUser.Role!.ToLowerInvariant());
            return Ok($"Welcome {LoggedInUser.Username}!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("makeaccount")]
    public async Task<IActionResult> MakeAccount([FromBody] User user)
    {
        if (user is null) return BadRequest("This is not a user.");
        if (user.Email == "" || user.Username == "" || user.Role == "" || user.Password == "" || user.Address == "") return BadRequest("Data is not complete.");
        if (!RightsFilter.validRoles.Contains(user.Role!.ToLowerInvariant())) return BadRequest("This Role is invalid!");
        if (user.Role.ToLowerInvariant() == "inventory manager" || user.Role.ToLowerInvariant() == "floor manager" || user.Role.ToLowerInvariant() == "operative" || user.Role.ToLowerInvariant() == "supervisor")
        {
            if (user.WarehouseId is null) return BadRequest("Data is not Complete for this Role! You need a Warehouse.");
        }
        bool IsUserWritenToJson = await _userServices.SaveUser(user);
        if (!IsUserWritenToJson) return BadRequest("Something went wrong!");
        return Ok("User registered");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (HttpContext.Session.GetInt32("UserId") is null) return BadRequest("You are not logged in!");
        int Id = (int)HttpContext.Session.GetInt32("UserId")!;
        User user = await _userServices.GetUser(Id);
        if (user != null)
        {
            HttpContext.Session.Clear();
            return Ok($"See you later {user.Username}!");
        }
        return BadRequest("No data found");
    }
}
