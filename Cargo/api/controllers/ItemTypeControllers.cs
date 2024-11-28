using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemTypeControllers : Controller
{
    private ItemTypeServices _itemTypeService;

    public ItemTypeControllers(ItemTypeServices itemTypeServices)
    {
        _itemTypeService = itemTypeServices;
    }

    [HttpGet("item-types")]
    public async Task<IActionResult> GetItemtypes() => Ok(await _itemTypeService.GetItemTypes());

    [HttpGet("item-type/{itemTypeId}")]
    public async Task<IActionResult> GetItemType(int itemTypeId)
    {
        if (itemTypeId == 0) return BadRequest("You can't use an empty id. ");
        ItemType? itemType = await _itemTypeService.GetItemType(itemTypeId);
        if (itemType is null) return BadRequest("Item type not found. ");
        return Ok(itemType);
    }

    [HttpPost("item-type")]
    public async Task<IActionResult> AddItemType([FromBody] ItemType itemType)
    {
        if (itemType is null || itemType.Name == "") return BadRequest("Not enough info. ");

        bool IsAdded = await _itemTypeService.AddItemType(itemType);
        if (!IsAdded) return BadRequest("Can't add item type. ");
        return Ok("Item type added. ");
    }

    [HttpPut("item-type")]
    public async Task<IActionResult> UpdateItemType([FromBody] ItemType itemType)
    {
        if (itemType is null || itemType.Id == 0) return BadRequest("Not enough info. ");

        bool IsUpdated = await _itemTypeService.UpdateItemType(itemType);
        if (!IsUpdated) return BadRequest("Item type can't be updated. ");
        return Ok("Item type updated. ");
    }

    [HttpDelete("item-type")]
    public async Task<IActionResult> RemoveItemType([FromQuery] int itemTypeId)
    {
        if (itemTypeId == 0) return BadRequest("Can't remove item type with empty id. ");

        bool IsRemoved = await _itemTypeService.RemoveItemType(itemTypeId);
        if (!IsRemoved) return BadRequest("Couldn't remove item type with given id. ");
        return Ok("Item type removed. ");
    }
}