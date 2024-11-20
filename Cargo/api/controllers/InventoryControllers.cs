using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class InventoryControllers : Controller
{
    InventoryServices IS;

    public InventoryControllers(InventoryServices invs)
    {
        IS = invs;
    }
}