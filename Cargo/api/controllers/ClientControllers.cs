using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ClientControllers : Controller
{
    ClientServices CS;

    public ClientControllers(ClientServices cs)
    {
        CS = cs;
    }
}