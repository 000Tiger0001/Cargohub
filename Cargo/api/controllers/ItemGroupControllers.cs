using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemGroupControllers : Controller
{
    ItemGroupServices IGS;

    public ItemGroupControllers(ItemGroupServices igs)
    {
        IGS = igs;
    }

    [HttpGet("get-item-groups")]
    public async Task<IActionResult> GetItemGroups()
    {
        List<ItemGroup> itemGroups = await IGS.GetItemGroups();
        return Ok(itemGroups);
    }

    [HttpGet("get-item-group")]
    public async Task<IActionResult> GetItemGroup([FromQuery] Guid itemGroupId)
    {
        if (itemGroupId == Guid.Empty) return BadRequest("You can't use an empty string. ");
        ItemGroup itemGroup = await IGS.GetItemGroup(itemGroupId);
        if (itemGroup is null) return BadRequest("Item group not found. ");
        return Ok(itemGroup);
    }

    [HttpPost("add-item-group")]
    public async Task<IActionResult> AddItemGroup([FromBody] ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Name == "") return BadRequest("Not enough info. ");

        bool IsAdded = await IGS.AddItemGroup(itemGroup);
        if (!IsAdded) return BadRequest("Can't add item group. ");
        return Ok("Item group added. ");
    }

    [HttpPut("update-item-group")]
    public async Task<IActionResult> UpdateItemGroup([FromBody] ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Id == Guid.Empty) return BadRequest("Not enough info. ");
        
        bool IsUpdated = await IGS.UpdateItemGroup(itemGroup);
        if(!IsUpdated) return BadRequest("Item group can't be updated. ");
        return Ok("Item group updated. ");
    }

    [HttpDelete("remove-item-group")]
    public async Task<IActionResult> RemoveItemGroup([FromQuery] Guid itemGroupId)
    {
        if (itemGroupId == Guid.Empty) return BadRequest("Can't remove item group with empty id. ");

        bool IsRemoved = await IGS.RemoveItemGroup(itemGroupId);
        if (!IsRemoved) return BadRequest("Couldn't remove item group with given id. ");
        return Ok("Item group removed. ");
    }
}