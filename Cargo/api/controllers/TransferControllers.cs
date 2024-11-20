using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class TransferControllers : Controller
{
    TransferServices TS;

    public TransferControllers(TransferServices ts)
    {
        TS = ts;
    }

}