using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemControllers : Controller
{
    ItemServices IS;

    public ItemControllers(ItemServices itemServices)
    {
        IS = itemServices;
    }
}