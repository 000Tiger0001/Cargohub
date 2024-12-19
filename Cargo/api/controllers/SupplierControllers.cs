using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class SupplierControllers : Controller
{
    private SupplierServices _supplierService;

    public SupplierControllers(SupplierServices supplierServices)
    {
        _supplierService = supplierServices;
    }

    [HttpGet("suppliers")]
    public async Task<IActionResult> GetSuppliers() => Ok(await _supplierService.GetSuppliers());

    [HttpGet("supplier/{supplierId}")]
    public async Task<IActionResult> GetSupplier(int supplierId)
    {
        if (supplierId <= 0) return BadRequest("Can't get supplier with this id. ");

        Supplier? supplier = await _supplierService.GetSupplier(supplierId);
        if (supplier is null) BadRequest("Supplier not found. ");
        return Ok(supplier);
    }

    [HttpPost("supplier")]
    public async Task<IActionResult> AddSupplier([FromBody] Supplier supplier)
    {
        if (supplier is null || supplier.Address == "" || supplier.AddressExtra == "" || supplier.City == "" || supplier.Code == "" || supplier.ContactName == "" || supplier.Country == "" || supplier.Name == "" || supplier.Phonenumber == "" || supplier.Province == "" || supplier.Reference == "" || supplier.ZipCode == "") return BadRequest("Not enough info given. ");

        bool IsAdded = await _supplierService.AddSupplier(supplier);
        if (!IsAdded) return BadRequest("Can't add supplier. ");
        return Ok("Supplier added. ");
    }

    [HttpPut("supplier")]
    public async Task<IActionResult> UpdateSupplier([FromBody] Supplier supplier)
    {
        if (supplier is null || supplier.Id <= 0) return BadRequest("Not enough given. ");

        bool IsUpdated = await _supplierService.UpdateSupplier(supplier);
        if (!IsUpdated) return BadRequest("Supplier can't be updated. ");
        return Ok("Supplier updated. ");
    }

    [HttpDelete("supplier/{supplierId}")]
    public async Task<IActionResult> RemoveSupplier(int supplierId)
    {
        if (supplierId <= 0) return BadRequest("Can't remove supplier with this id. ");

        bool IsRemoved = await _supplierService.RemoveSupplier(supplierId);
        if (!IsRemoved) return BadRequest("Supplier can't be removed. ");
        return Ok("Supplier removed. ");
    }
}