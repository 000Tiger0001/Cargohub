using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class WarehouseControllers : Controller
{
    WarehouseServices WS;

    public WarehouseControllers(WarehouseServices ws)
    {
        WS = ws;
    }

    [HttpGet("warehouses")]
    public async Task<IActionResult> GetWarehouses() => Ok(await WS.GetWarehouses());

    [HttpGet("warehouse")]
    public async Task<IActionResult> GetWarehouse([FromQuery] Guid warehouseId)
    {
        Warehouse warehouse = await WS.GetWarehouse(warehouseId);
        if (warehouse is not null) return Ok(warehouse);
        return BadRequest("No warehouse found with this id. ");
    }

    [HttpPost("add-warehouse")]
    public async Task<IActionResult> AddWarehouse([FromBody] Warehouse warehouse)
    {
        if (warehouse is null || warehouse.Code == "" || warehouse.Name == "" || warehouse.Address == "" || warehouse.Zip == "" || warehouse.City == "" || warehouse.Province == "" || warehouse.Country == "") return BadRequest("Not enough info given");

        bool IsAdded = await WS.AddWarehouse(warehouse);
        if (!IsAdded) return BadRequest("This warehouse cannot be added. ");
        return Ok("Warehouse added. ");
    }

    [HttpPut("update-warehouse")]
    public async Task<IActionResult> UpdateWarehouse([FromBody] Warehouse warehouse)
    {
        if (warehouse is null || warehouse.Id == Guid.Empty) return BadRequest("Warehouse doesn't have an id. ");

        bool IsUpdated = await WS.UpdateWarehouse(warehouse);
        if (!IsUpdated) return BadRequest("Warehouse couldn't be updated. ");
        return Ok("Warehouse updated. ");
    }

    [HttpDelete("remove-warehouse")]
    public async Task<IActionResult> RemoveWarehouse([FromQuery] Guid warehouseId)
    {
        if (warehouseId == Guid.Empty) return BadRequest("Can't remove warehouse with empty id. ");

        bool IsRemoved = await WS.RemoveWarehouse(warehouseId);
        if (!IsRemoved) return BadRequest("Warehouse couldn't be removed. ");
        return Ok("Warehouse removed. ");
    }
}