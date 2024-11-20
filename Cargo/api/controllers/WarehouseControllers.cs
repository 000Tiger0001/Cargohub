using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class WarehouseControllers : Controller
{
    private WarehouseServices _warehouseService;

    public WarehouseControllers(WarehouseServices warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet("warehouses")]
    public async Task<IActionResult> GetWarehouses() => Ok(await _warehouseService.GetWarehouses());

    [HttpGet("warehouse")]
    public async Task<IActionResult> GetWarehouse([FromQuery] int warehouseId)
    {
        Warehouse? warehouse = await _warehouseService.GetWarehouse(warehouseId)!;
        if (warehouse is null) return BadRequest("No warehouse found with this id. ");
        return Ok(warehouse);
    }

    [HttpPost("add-warehouse")]
    public async Task<IActionResult> AddWarehouse([FromBody] Warehouse warehouse)
    {
        if (warehouse is null || warehouse.Code == "" || warehouse.Name == "" || warehouse.Address == "" || warehouse.Zip == "" || warehouse.City == "" || warehouse.Province == "" || warehouse.Country == "") return BadRequest("Not enough info given");

        bool IsAdded = await _warehouseService.AddWarehouse(warehouse);
        if (!IsAdded) return BadRequest("This warehouse cannot be added. ");
        return Ok("Warehouse added. ");
    }

    [HttpPut("update-warehouse")]
    public async Task<IActionResult> UpdateWarehouse([FromBody] Warehouse warehouse)
    {
        if (warehouse is null || warehouse.Id == 0) return BadRequest("Warehouse doesn't have an id. ");

        bool IsUpdated = await _warehouseService.UpdateWarehouse(warehouse);
        if (!IsUpdated) return BadRequest("Warehouse couldn't be updated. ");
        return Ok("Warehouse updated. ");
    }

    [HttpDelete("remove-warehouse")]
    public async Task<IActionResult> RemoveWarehouse([FromQuery] int warehouseId)
    {
        if (warehouseId == 0) return BadRequest("Can't remove warehouse with empty id. ");

        bool IsRemoved = await _warehouseService.RemoveWarehouse(warehouseId);
        if (!IsRemoved) return BadRequest("Warehouse couldn't be removed. ");
        return Ok("Warehouse removed. ");
    }
}