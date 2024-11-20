using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class InventoryControllers : Controller
{
    InventoryServices IS;

    public InventoryControllers(InventoryServices invs)
    {
        IS = invs;
    }

    [HttpGet("inventories")]
    public async Task<IActionResult> GetInventories() => Ok(await IS.GetInventories());

    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventory([FromQuery] Guid inventoryId)
    {
        if (inventoryId == Guid.Empty) return BadRequest("Cannot proccess empty id. ");
        Inventory inventory = await IS.GetInventory(inventoryId);
        if (inventory is null) return BadRequest("Inventory does not exist. ");
        return Ok(inventory);
    }

    [HttpGet("get-inventories-for-item")]
    public async Task<IActionResult> GetInventoriesforItem([FromQuery] Guid itemId)
    {
        if (itemId == Guid.Empty) return BadRequest("Cannot proccess empty id. ");
        List<Inventory> inventories = await IS.GetInventoriesforItem(itemId);
        if (inventories.Count <= 0) return BadRequest("No inventory found with this item id. ");
        return Ok(inventories);
    }

    [HttpGet("get-inventory-totals-for-item")]
    public async Task<IActionResult> GetInventoryTotalsForItem([FromQuery] Guid itemId)
    {
        if (itemId == Guid.Empty) return BadRequest("Cannot proccess empty id. ");
        Dictionary<string, int> result = await IS.GetInventoryTotalsForItem(itemId);
        if (result["total_expected"] == 0 && result["total_ordered"] == 0 && result["total_allocated"] == 0 && result["total_available"] == 0) return BadRequest("Id is invalid. ");
        return Ok(result);
    }

    [HttpPost("add-inventory")]
    public async Task<IActionResult> AddInventory([FromBody] Inventory inventory)
    {
        if (inventory is null || inventory.Description == default || inventory.ItemReference == default || inventory.ItemId == Guid.Empty) BadRequest("Not enough info given. ");
        
        bool IsAdded = await IS.AddInventory(inventory!);
        if (!IsAdded) return BadRequest("Inventory not saved. ");
        return Ok("Inventory added. ");
    }

    [HttpPut("update-inventory")]
    public async Task<IActionResult> UpdateInventory([FromBody] Inventory inventory)
    {
        if (inventory is null || inventory.Id == Guid.Empty || inventory.ItemId == Guid.Empty) BadRequest("Not enough info given. ");
        
        bool IsUpdated = await IS.UpdateInventory(inventory!);
        if (!IsUpdated) return BadRequest("Inventory not updated. ");
        return Ok("Inventory updated. ");
    }

    [HttpDelete("remove-inventory")]
    public async Task<IActionResult> RemoveInventory([FromQuery] Guid inventoryId)
    {
        if (inventoryId == Guid.Empty) return BadRequest("Cannot proccess empty id. ");

        bool IsRemoved = await IS.RemoveInventory(inventoryId);
        if (!IsRemoved) BadRequest("Inventory with given id doesn't exist. ");
        return Ok("Inventory removed. ");
    }
}