using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class InventoryControllers : Controller
{
    private InventoryServices _inventoryService;

    public InventoryControllers(InventoryServices inventoryServices)
    {
        _inventoryService = inventoryServices;
    }

    [HttpGet("inventories")]
    public async Task<IActionResult> GetInventories() => Ok(await _inventoryService.GetInventories());

    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventory([FromQuery] int inventoryId)
    {
        if (inventoryId == 0) return BadRequest("Cannot proccess empty id. ");
        Inventory? inventory = await _inventoryService.GetInventory(inventoryId);
        if (inventory is null) return BadRequest("Inventory does not exist. ");
        return Ok(inventory);
    }

    [HttpGet("get-inventories-for-item")]
    public async Task<IActionResult> GetInventoriesforItem([FromQuery] int itemId)
    {
        if (itemId ==0) return BadRequest("Cannot proccess empty id. ");
        List<Inventory> inventories = await _inventoryService.GetInventoriesforItem(itemId);
        if (inventories.Count <= 0) return BadRequest("No inventory found with this item id. ");
        return Ok(inventories);
    }

    [HttpGet("get-inventory-totals-for-item")]
    public async Task<IActionResult> GetInventoryTotalsForItem([FromQuery] int itemId)
    {
        if (itemId == 0) return BadRequest("Cannot proccess empty id. ");
        Dictionary<string, int> result = await _inventoryService.GetInventoryTotalsForItem(itemId);
        if (result["total_expected"] == 0 && result["total_ordered"] == 0 && result["total_allocated"] == 0 && result["total_available"] == 0) return BadRequest("Id is invalid. ");
        return Ok(result);
    }

    [HttpPost("add-inventory")]
    public async Task<IActionResult> AddInventory([FromBody] Inventory inventory)
    {
        if (inventory is null || inventory.Description == default || inventory.ItemReference == default || inventory.ItemId == 0) BadRequest("Not enough info given. ");
        
        bool IsAdded = await _inventoryService.AddInventory(inventory!);
        if (!IsAdded) return BadRequest("Inventory not saved. ");
        return Ok("Inventory added. ");
    }

    [HttpPut("update-inventory")]
    public async Task<IActionResult> UpdateInventory([FromBody] Inventory inventory)
    {
        if (inventory is null || inventory.Id == 0 || inventory.ItemId == 0) BadRequest("Not enough info given. ");
        
        bool IsUpdated = await _inventoryService.UpdateInventory(inventory!);
        if (!IsUpdated) return BadRequest("Inventory not updated. ");
        return Ok("Inventory updated. ");
    }

    [HttpDelete("remove-inventory")]
    public async Task<IActionResult> RemoveInventory([FromQuery] int inventoryId)
    {
        if (inventoryId == 0) return BadRequest("Cannot proccess empty id. ");

        bool IsRemoved = await _inventoryService.RemoveInventory(inventoryId);
        if (!IsRemoved) BadRequest("Inventory with given id doesn't exist. ");
        return Ok("Inventory removed. ");
    }
}