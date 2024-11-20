using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemGroupControllers : Controller
{
    ItemGroupServices IGS;

    public ItemGroupControllers(ItemGroupServices igs)
    {
        IGS = igs;
    }
}