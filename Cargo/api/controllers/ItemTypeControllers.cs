using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemTypeControllers : Controller
{
    ItemTypeServices ITS;

    public ItemTypeControllers(ItemTypeServices its)
    {
        ITS = its;
    }

    [HttpGet("get-item-types")]
    public async Task<IActionResult> GetItemtypes() => Ok(await ITS.GetItemTypes());

    [HttpGet("get-item-type")]
    public async Task<IActionResult> GetItemType([FromQuery] Guid itemTypeId)
    {
        if (itemTypeId == Guid.Empty) return BadRequest("You can't use an empty string. ");
        ItemType itemType = await ITS.GetItemType(itemTypeId);
        if (itemType is null) return BadRequest("Item type not found. ");
        return Ok(itemType);
    }

    [HttpPost("add-item-type")]
    public async Task<IActionResult> AddItemType([FromBody] ItemType itemType)
    {
        if (itemType is null || itemType.Name == "") return BadRequest("Not enough info. ");

        bool IsAdded = await ITS.AddItemType(itemType);
        if (!IsAdded) return BadRequest("Can't add item type. ");
        return Ok("Item type added. ");
    }

    [HttpPut("update-item-type")]
    public async Task<IActionResult> UpdateItemType([FromBody] ItemType itemType)
    {
        if (itemType is null || itemType.Id == Guid.Empty) return BadRequest("Not enough info. ");

        bool IsUpdated = await ITS.UpdateItemType(itemType);
        if (!IsUpdated) return BadRequest("Item type can't be updated. ");
        return Ok("Item type updated. ");
    }

    [HttpDelete("remove-item-type")]
    public async Task<IActionResult> RemoveItemType([FromQuery] Guid itemTypeId)
    {
        if (itemTypeId == Guid.Empty) return BadRequest("Can't remove item type with empty id. ");

        bool IsRemoved = await ITS.RemoveItemType(itemTypeId);
        if (!IsRemoved) return BadRequest("Couldn't remove item type with given id. ");
        return Ok("Item type removed. ");
    }
}