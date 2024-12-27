using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemControllers : Controller
{
    private ItemServices _itemService;

    public ItemControllers(ItemServices itemServices)
    {
        _itemService = itemServices;
    }

    [HttpGet("items")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Analyst", "Logistics", "Sales", "Operative", "Supervisor"])]
    public async Task<IActionResult> GetItems()
    {
        if (HttpContext.Session.GetString("Role") == "Operative" || HttpContext.Session.GetString("Role") == "Supervisor")
        {
            return Ok(await _itemService.GetItemsforUser((int)HttpContext.Session.GetInt32("UserId")!));
        }
        return Ok(await _itemService.GetItems());
    }

    [HttpGet("item/{itemId}")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Operative", "Floor Manager", "Supervisor", "Analyst", "Logistics", "Sales"])]
    public async Task<IActionResult> GetItem(int itemId)
    {
        if (itemId <= 0) return BadRequest("Can't proccess this id. ");

        Item? item = await _itemService.GetItem(itemId);
        if (item is null) return BadRequest("Item with given id doesn't exist. ");
        return Ok(item);
    }

    [HttpPost("item")]
    [RightsFilter(["Admin", "Logistics", "Warehouse Manager", "Sales"])]
    public async Task<IActionResult> AddItem([FromBody] Item item)
    {
        if (item is null || item.Code == "" || item.CommodityCode == "" || item.Description == "" || item.ItemGroupId == default || item.ItemLineId == default || item.ItemTypeId == default || item.ModelNumber == "" || item.UpcCode == "" || item.ShortDescription == "") return BadRequest("Not enough info given. ");

        bool IsAdded = await _itemService.AddItem(item);
        if (!IsAdded) return BadRequest("Item couldn't be added. ");
        return Ok("Item added. ");
    }

    [HttpPut("item")]
    [RightsFilter(["Admin", "Warehouse Manager", "Sales"])]
    public async Task<IActionResult> UpdateItem([FromBody] Item item)
    {
        if (item is null || item.Id <= 0) return BadRequest("Not enough info given. ");

        bool IsUpdated = await _itemService.UpdateItem(item);
        if (!IsUpdated) return BadRequest("Item couldn't be updated. ");
        return Ok("Item updated. ");
    }

    [HttpDelete("item/{itemId}")]
    [RightsFilter(["Admin"])]
    public async Task<IActionResult> RemoveItem(int itemId)
    {
        if (itemId <= 0) return BadRequest("Can't proccess this id. ");

        bool IsRemoved = await _itemService.RemoveItem(itemId);
        if (!IsRemoved) return BadRequest("Item couldn't be removed. ");
        return Ok("Item removed. ");
    }

    [HttpGet("item-type/{itemTypeId}/items")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Analyst", "Logistics", "Sales"])]
    public async Task<IActionResult> GetItemsForItemType(int itemTypeId)
    {
        if (itemTypeId <= 0) return BadRequest("Can't proccess this id. ");

        List<Item> items = await _itemService.GetItemsForItemType(itemTypeId);
        if (items.Count <= 0) return BadRequest("No items found with given item type id. ");
        return Ok(items);
    }

    [HttpGet("item-line/{itemLineId}/items")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Analyst", "Logistics", "Sales"])]
    public async Task<IActionResult> GetItemsForItemLine(int itemLineId)
    {
        if (itemLineId <= 0) return BadRequest("Can't proccess this id. ");

        List<Item> items = await _itemService.GetItemsForItemLine(itemLineId);
        if (items.Count <= 0) return BadRequest("No items found with given item line id. ");
        return Ok(items);
    }

    [HttpGet("item-group/{itemGroupId}/items")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Analyst", "Logistics", "Sales"])]
    public async Task<IActionResult> GetItemsForItemGroup(int itemGroupId)
    {
        if (itemGroupId <= 0) return BadRequest("Can't proccess this id. ");

        List<Item> items = await _itemService.GetItemsForItemGroup(itemGroupId);
        if (items.Count <= 0) return BadRequest("No items found with given item group id. ");
        return Ok(items);
    }
}