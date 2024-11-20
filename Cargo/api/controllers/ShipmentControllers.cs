using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ShipmentControllers : Controller
{
    ShipmentServices SS;

    public ShipmentControllers(ShipmentServices ss)
    {
        SS = ss;
    }
}