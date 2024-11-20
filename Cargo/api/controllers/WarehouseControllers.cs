using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class WarehouseControllers : Controller
{
    WarehouseServices WS;

    public WarehouseControllers(WarehouseServices ws)
    {
        WS = ws;
    }
}