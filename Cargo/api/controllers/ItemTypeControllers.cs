using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemTypeControllers : Controller
{
    ItemTypeServices ITS;

    public ItemTypeControllers(ItemTypeServices its)
    {
        ITS = its;
    }


}