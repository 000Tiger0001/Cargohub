using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemTypeControllers : Controller
{
    private ItemTypeServices _itemTypeService;

    public ItemTypeControllers(ItemTypeAccess itemTypeAccess)
    {
        _itemTypeService = new(itemTypeAccess, false);
    }

    [HttpGet("get-item-types")]
    public async Task<IActionResult> GetItemtypes() => Ok(await _itemTypeService.GetItemTypes());

    [HttpGet("get-item-type")]
    public async Task<IActionResult> GetItemType([FromQuery] int itemTypeId)
    {
        if (itemTypeId == 0) return BadRequest("You can't use an empty id. ");
        ItemType? itemType = await _itemTypeService.GetItemType(itemTypeId);
        if (itemType is null) return BadRequest("Item type not found. ");
        return Ok(itemType);
    }

    [HttpPost("add-item-type")]
    public async Task<IActionResult> AddItemType([FromBody] ItemType itemType)
    {
        if (itemType is null || itemType.Name == "") return BadRequest("Not enough info. ");

        bool IsAdded = await _itemTypeService.AddItemType(itemType);
        if (!IsAdded) return BadRequest("Can't add item type. ");
        return Ok("Item type added. ");
    }

    [HttpPut("update-item-type")]
    public async Task<IActionResult> UpdateItemType([FromBody] ItemType itemType)
    {
        if (itemType is null || itemType.Id == 0) return BadRequest("Not enough info. ");

        bool IsUpdated = await _itemTypeService.UpdateItemType(itemType);
        if (!IsUpdated) return BadRequest("Item type can't be updated. ");
        return Ok("Item type updated. ");
    }

    [HttpDelete("remove-item-type")]
    public async Task<IActionResult> RemoveItemType([FromQuery] int itemTypeId)
    {
        if (itemTypeId == 0) return BadRequest("Can't remove item type with empty id. ");

        bool IsRemoved = await _itemTypeService.RemoveItemType(itemTypeId);
        if (!IsRemoved) return BadRequest("Couldn't remove item type with given id. ");
        return Ok("Item type removed. ");
    }
}