using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemGroupControllers : Controller
{
    private ItemGroupServices _itemGroupService;

    public ItemGroupControllers(ItemGroupAccess itemGroupAccess)
    {
        _itemGroupService = new(itemGroupAccess, false);
    }

    [HttpGet("get-item-groups")]
    public async Task<IActionResult> GetItemGroups() => Ok(await _itemGroupService.GetItemGroups());

    [HttpGet("get-item-group")]
    public async Task<IActionResult> GetItemGroup([FromQuery] int itemGroupId)
    {
        if (itemGroupId == 0) return BadRequest("You can't use an empty string. ");
        ItemGroup? itemGroup = await _itemGroupService.GetItemGroup(itemGroupId);
        if (itemGroup is null) return BadRequest("Item group not found. ");
        return Ok(itemGroup);
    }

    [HttpPost("add-item-group")]
    public async Task<IActionResult> AddItemGroup([FromBody] ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Name == "") return BadRequest("Not enough info. ");

        bool IsAdded = await _itemGroupService.AddItemGroup(itemGroup);
        if (!IsAdded) return BadRequest("Can't add item group. ");
        return Ok("Item group added. ");
    }

    [HttpPut("update-item-group")]
    public async Task<IActionResult> UpdateItemGroup([FromBody] ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Id == 0) return BadRequest("Not enough info. ");
        
        bool IsUpdated = await _itemGroupService.UpdateItemGroup(itemGroup);
        if(!IsUpdated) return BadRequest("Item group can't be updated. ");
        return Ok("Item group updated. ");
    }

    [HttpDelete("remove-item-group")]
    public async Task<IActionResult> RemoveItemGroup([FromQuery] int itemGroupId)
    {
        if (itemGroupId == 0) return BadRequest("Can't remove item group with empty id. ");

        bool IsRemoved = await _itemGroupService.RemoveItemGroup(itemGroupId);
        if (!IsRemoved) return BadRequest("Couldn't remove item group with given id. ");
        return Ok("Item group removed. ");
    }
}