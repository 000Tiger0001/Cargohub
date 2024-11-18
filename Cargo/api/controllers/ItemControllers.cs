using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemControllers : Controller
{
    ItemServices IS;

    public ItemControllers(ItemServices itemServices)
    {
        IS = itemServices;
    }

    [HttpGet("get-items")]
    public async Task<IActionResult> GetItems() => Ok(await IS.GetItems());

    [HttpGet("get-item")]
    public async Task<IActionResult> GetItem([FromQuery] Guid itemId)
    {
        if (itemId == Guid.Empty) return BadRequest("Id can't be empty. ");
        
        Item item = await IS.GetItem(itemId);
        if (item is null) return BadRequest("Item with given id doesn't exist. ");
        return Ok(item);
    }

    [HttpGet("get-items-for-item-group")]
    public async Task<IActionResult> GetItemsForItemGroup([FromQuery] Guid itemGroupId)
    {
        if (itemGroupId == Guid.Empty) return BadRequest("Given id can't be empty. ");

        List<Item> items = await IS.GetItemsForItemGroup(itemGroupId);
        if (items.Count <= 0) return BadRequest("No items found with given item group id. ");
        return Ok(items);
    }

    [HttpGet("get-items-for-item-line")]
    public async Task<IActionResult> GetItemsForItemLine([FromQuery] Guid itemLineId)
    {
        if (itemLineId == Guid.Empty) return BadRequest("Given id can't be empty. ");

        List<Item> items = await IS.GetItemsForItemLine(itemLineId);
        if (items.Count <= 0) return BadRequest("No items found with given item line id. ");
        return Ok(items);
    }

    [HttpGet("get-items-for-item-type")]
    public async Task<IActionResult> GetItemsForItemType([FromQuery] Guid itemTypeId)
    {
        if (itemTypeId == Guid.Empty) return BadRequest("Given id can't be empty. ");

        List<Item> items = await IS.GetItemsForItemType(itemTypeId);
        if (items.Count <= 0) return BadRequest("No items found with given item type id. ");
        return Ok(items);
    }

    [HttpPost("add-item")]
    public async Task<IActionResult> AddItem([FromBody] Item item)
    {
        if (item is null || item.Code == "" || item.CommodityCode == "" || item.Description == "" || item.ItemGroupId == default || item.ItemLineId == default || item.ItemTypeId == default || item.ModelNumber == "" || item.UpcCode == "" || item.ShortDescription == "") return BadRequest("Not enough info given. ");

        bool IsAdded = await IS.AddItem(item);
        if (!IsAdded) return BadRequest("Item couldn't be added. ");
        return Ok("Item added. ");
    }

    [HttpPut("update-item")]
    public async Task<IActionResult> UpdateItem([FromBody] Item item)
    {
        if (item is null || item.Id == Guid.Empty) return BadRequest("Not enough info given. ");

        bool IsUpdated = await IS.UpdateItem(item);
        if (!IsUpdated) return BadRequest("Item couldn't be updated. ");
        return Ok("Item updated. ");
    }

    [HttpDelete("remove-item")]
    public async Task<IActionResult> RemoveItem([FromQuery] Guid itemId)
    {
        if (itemId == Guid.Empty) return BadRequest("Given id isn't valid");

        bool IsRemoved = await IS.RemoveItem(itemId);
        if (!IsRemoved) return BadRequest("Item couldn't be removed. ");
        return Ok("Item removed. ");
    }
}