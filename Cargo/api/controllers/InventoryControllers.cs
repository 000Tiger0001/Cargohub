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
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Analyst", "Logistics", "Sales", "Operative", "Supervisor"])]
    public async Task<IActionResult> GetInventories()
    {
        if (HttpContext.Session.GetString("Role") == "Operative" || HttpContext.Session.GetString("Role") == "Supervisor") return Ok(await _inventoryService.GetInventoriesforUser((int)HttpContext.Session.GetInt32("UserId")!));
        return Ok(await _inventoryService.GetInventories());
    }

    [HttpGet("inventory/{inventoryId}")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Analyst", "Logistics", "Sales", "Operative", "Supervisor"])]
    public async Task<IActionResult> GetInventory(int inventoryId)
    {
        if (inventoryId <= 0) return BadRequest("Cannot proccess this id. ");
        Inventory? inventory = await _inventoryService.GetInventory(inventoryId);
        if (inventory is null) return BadRequest("Inventory does not exist. ");
        return Ok(inventory);
    }

    [HttpGet("inventories/item/{itemId}")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Analyst", "Logistics", "Sales"])]
    public async Task<IActionResult> GetInventoriesforItem(int itemId)
    {
        if (itemId <= 0) return BadRequest("Can't proccess this id. ");
        List<Inventory> inventories = await _inventoryService.GetInventoriesforItem(itemId);
        if (inventories.Count <= 0) return BadRequest("No inventory found with this item id. ");
        return Ok(inventories);
    }

    [HttpGet("inventories/item/{itemId}/totals")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Analyst", "Logistics", "Sales"])]
    public async Task<IActionResult> GetInventoryTotalsForItem(int itemId)
    {
        if (itemId <= 0) return BadRequest("Can't proccess this id. ");
        Dictionary<string, int> result = await _inventoryService.GetInventoryTotalsForItem(itemId);
        if (result["total_expected"] == 0 && result["total_ordered"] == 0 && result["total_allocated"] == 0 && result["total_available"] == 0) return BadRequest("Id is invalid. ");
        return Ok(result);
    }

    [HttpPost("inventory")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Logistics"])]
    public async Task<IActionResult> AddInventory([FromBody] Inventory inventory)
    {
        if (inventory is null || inventory.Description == default || inventory.ItemReference == default || inventory.ItemId == 0) BadRequest("Not enough info given. ");

        bool IsAdded = await _inventoryService.AddInventory(inventory!);
        if (!IsAdded) return BadRequest("Inventory not saved. ");
        return Ok("Inventory added. ");
    }

    [HttpPut("inventory")]
    [RightsFilter(["Admin", "Warehouse Manager", "Logistics", "Inventory Manager"])]
    public async Task<IActionResult> UpdateInventory([FromBody] Inventory inventory)
    {
        if (inventory is null || inventory.Id == 0 || inventory.ItemId == 0) BadRequest("Not enough info given. ");

        bool IsUpdated = await _inventoryService.UpdateInventory(inventory!);
        if (!IsUpdated) return BadRequest("Inventory not updated. ");
        return Ok("Inventory updated. ");
    }

    [HttpDelete("inventory/{inventoryId}")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager"])]
    public async Task<IActionResult> RemoveInventory(int inventoryId)
    {
        if (inventoryId <= 0) return BadRequest("Can't proccess this id. ");

        bool IsRemoved = await _inventoryService.RemoveInventory(inventoryId);
        if (!IsRemoved) BadRequest("Inventory with given id doesn't exist. ");
        return Ok("Inventory removed. ");
    }
}