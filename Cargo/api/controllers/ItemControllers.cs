using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemControllers : Controller
{
    private ItemServices _itemService;

    public ItemControllers(ItemAccess itemAccess)
    {
        _itemService = new(itemAccess, false);
    }

    [HttpGet("get-items")]
    public async Task<IActionResult> GetItems() => Ok(await _itemService.GetItems());

    [HttpGet("get-item")]
    public async Task<IActionResult> GetItem([FromQuery] int itemId)
    {
        if (itemId == 0) return BadRequest("Id can't be empty. ");
        
        Item? item = await _itemService.GetItem(itemId);
        if (item is null) return BadRequest("Item with given id doesn't exist. ");
        return Ok(item);
    }

    [HttpGet("get-items-for-item-group")]
    public async Task<IActionResult> GetItemsForItemGroup([FromQuery] int itemGroupId)
    {
        if (itemGroupId == 0) return BadRequest("Given id can't be empty. ");

        List<Item> items = await _itemService.GetItemsForItemGroup(itemGroupId);
        if (items.Count <= 0) return BadRequest("No items found with given item group id. ");
        return Ok(items);
    }

    [HttpGet("get-items-for-item-line")]
    public async Task<IActionResult> GetItemsForItemLine([FromQuery] int itemLineId)
    {
        if (itemLineId == 0) return BadRequest("Given id can't be empty. ");

        List<Item> items = await _itemService.GetItemsForItemLine(itemLineId);
        if (items.Count <= 0) return BadRequest("No items found with given item line id. ");
        return Ok(items);
    }

    [HttpGet("get-items-for-item-type")]
    public async Task<IActionResult> GetItemsForItemType([FromQuery] int itemTypeId)
    {
        if (itemTypeId == 0) return BadRequest("Given id can't be empty. ");

        List<Item> items = await _itemService.GetItemsForItemType(itemTypeId);
        if (items.Count <= 0) return BadRequest("No items found with given item type id. ");
        return Ok(items);
    }

    [HttpPost("add-item")]
    public async Task<IActionResult> AddItem([FromBody] Item item)
    {
        if (item is null || item.Code == "" || item.CommodityCode == "" || item.Description == "" || item.ItemGroupId == default || item.ItemLineId == default || item.ItemTypeId == default || item.ModelNumber == "" || item.UpcCode == "" || item.ShortDescription == "") return BadRequest("Not enough info given. ");

        bool IsAdded = await _itemService.AddItem(item);
        if (!IsAdded) return BadRequest("Item couldn't be added. ");
        return Ok("Item added. ");
    }

    [HttpPut("update-item")]
    public async Task<IActionResult> UpdateItem([FromBody] Item item)
    {
        if (item is null || item.Id == 0) return BadRequest("Not enough info given. ");

        bool IsUpdated = await _itemService.UpdateItem(item);
        if (!IsUpdated) return BadRequest("Item couldn't be updated. ");
        return Ok("Item updated. ");
    }

    [HttpDelete("remove-item")]
    public async Task<IActionResult> RemoveItem([FromQuery] int itemId)
    {
        if (itemId == 0) return BadRequest("Given id isn't valid");

        bool IsRemoved = await _itemService.RemoveItem(itemId);
        if (!IsRemoved) return BadRequest("Item couldn't be removed. ");
        return Ok("Item removed. ");
    }
}