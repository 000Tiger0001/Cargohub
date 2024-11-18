using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class SupplierControllers : Controller
{
    private SupplierServices _SS;

    public SupplierControllers(SupplierServices ss)
    {
        _SS = ss;
    }

    [HttpGet("suppliers")]
    public async Task<IActionResult> GetSuppliers() => Ok(await _SS.GetSuppliers());

    [HttpGet("supplier")]
    public async Task<IActionResult> GetSupplier([FromQuery] Guid supplierId)
    {
        if (supplierId == Guid.Empty) return BadRequest("Can't get supplier with empty id. ");

        Supplier supplier = await _SS.GetSupplier(supplierId);
        if (supplier is null) BadRequest("Supplier not found. ");
        return Ok(supplier);
    }

    [HttpPost("add-supplier")]
    public async Task<IActionResult> AddSupplier([FromBody] Supplier supplier)
    {
        if (supplier is null || supplier.Address == "" || supplier.AddressExtra == "" || supplier.City == "" || supplier.Code == "" || supplier.ContactName == "" || supplier.Country == "" || supplier.Name == "" || supplier.Phonenumber == "" || supplier.Province == "" || supplier.Reference == "" || supplier.ZipCode == "") return BadRequest("Not enough info given. ");

        bool IsAdded = await _SS.AddSupplier(supplier);
        if (!IsAdded) return BadRequest("Can't add supplier. ");
        return Ok("Supplier added. ");
    }

    public async Task<IActionResult> UpdateSupplier([FromBody] Supplier supplier)
    {
        if (supplier is null || supplier.Id == Guid.Empty) return BadRequest("Not enough given. ");

        bool IsUpdated = await _SS.UpdateSupplier(supplier);
        if (!IsUpdated) return BadRequest("Supplier can't be updated. ");
        return Ok("Supplier updated. ");
    }

    public async Task<IActionResult> RemoveSupplier([FromQuery] Guid supplierId)
    {
        if (supplierId == Guid.Empty) return BadRequest("Can't remove supplier with empty id. ");

        bool IsRemoved = await _SS.RemoveSupplier(supplierId);
        if (!IsRemoved) return BadRequest("Supplier can't be removed. ");
        return Ok("Supplier removed. ");
    }
}