using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class UserControllers : Controller
{
    private readonly UserServices _userServices;

    public UserControllers(UserServices userServices)
    {
        _userServices = userServices;
    }


}
