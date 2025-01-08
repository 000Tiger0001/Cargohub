using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemGroupControllers : Controller
{
    private ItemGroupServices _itemGroupService;

    public ItemGroupControllers(ItemGroupServices itemGroupServices)
    {
        _itemGroupService = itemGroupServices;
    }

    [HttpGet("item-groups")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Sales", "Logistics", "Analyst"])]
    public async Task<IActionResult> GetItemGroups() => Ok(await _itemGroupService.GetItemGroups());

    [HttpGet("item-group/{itemGroupId}")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Sales", "Logistics", "Analyst"])]
    public async Task<IActionResult> GetItemGroup(int itemGroupId)
    {
        if (itemGroupId <= 0) return BadRequest("You can't use an this id. ");
        ItemGroup? itemGroup = await _itemGroupService.GetItemGroup(itemGroupId);
        if (itemGroup is null) return BadRequest("Item group not found. ");
        return Ok(itemGroup);
    }

    [HttpPost("item-group")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager"])]
    public async Task<IActionResult> AddItemGroup([FromBody] ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Name == "") return BadRequest("Not enough info. ");

        bool IsAdded = await _itemGroupService.AddItemGroup(itemGroup);
        if (!IsAdded) return BadRequest("Can't add item group. ");
        return Ok("Item group added. ");
    }

    [HttpPut("item-group")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager"])]
    public async Task<IActionResult> UpdateItemGroup([FromBody] ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Id <= 0) return BadRequest("Not enough info. ");

        bool IsUpdated = await _itemGroupService.UpdateItemGroup(itemGroup);
        if (!IsUpdated) return BadRequest("Item group can't be updated. ");
        return Ok("Item group updated. ");
    }

    [HttpDelete("item-group/{itemGroupId}")]
    [RightsFilter(["Admin"])]
    public async Task<IActionResult> RemoveItemGroup(int itemGroupId)
    {
        if (itemGroupId <= 0) return BadRequest("Can't remove item group with this id. ");

        bool IsRemoved = await _itemGroupService.RemoveItemGroup(itemGroupId);
        if (!IsRemoved) return BadRequest("Couldn't remove item group with given id. ");
        return Ok("Item group removed. ");
    }
}