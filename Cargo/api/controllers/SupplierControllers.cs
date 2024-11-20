using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class SupplierControllers : Controller
{
    private SupplierServices SS;

    public SupplierControllers(SupplierServices ss)
    {
        SS = ss;
    }
}