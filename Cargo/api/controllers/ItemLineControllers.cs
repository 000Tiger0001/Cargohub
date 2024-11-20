using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemLineControllers : Controller
{
    ItemLineServices ILS;

    public ItemLineControllers(ItemLineServices ils)
    {
        ILS = ils;
    }


}