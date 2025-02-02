using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemLineControllers : Controller
{
    private ItemLineServices _itemLineService;

    public ItemLineControllers(ItemLineServices itemLineServices)
    {
        _itemLineService = itemLineServices;
    }

    [HttpGet("item-lines")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Sales", "Logistics", "Analyst"])]
    public async Task<IActionResult> GetItemLines() => Ok(await _itemLineService.GetItemLines());

    [HttpGet("item-line/{itemLineId}")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Sales", "Logistics", "Analyst"])]
    public async Task<IActionResult> GetItemLine(int itemLineId)
    {
        if (itemLineId <= 0) return BadRequest("You can't use an this id. ");
        ItemLine? itemLine = await _itemLineService.GetItemLine(itemLineId);
        if (itemLine is null) return BadRequest("Item line not found. ");
        return Ok(itemLine);
    }

    [HttpPost("item-line")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager"])]
    public async Task<IActionResult> AddItemLine([FromBody] ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Name == "") return BadRequest("Not enough info. ");

        bool IsAdded = await _itemLineService.AddItemLine(itemLine);
        if (!IsAdded) return BadRequest("Can't add item line. ");
        return Ok("Item line added. ");
    }

    [HttpPut("item-line")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager"])]
    public async Task<IActionResult> UpdateItemLine([FromBody] ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Id <= 0) return BadRequest("Not enough info. ");

        bool IsUpdated = await _itemLineService.UpdateItemLine(itemLine);
        if (!IsUpdated) return BadRequest("Item line can't be updated. ");
        return Ok("Item line updated. ");
    }

    [HttpDelete("item-line/{itemLineId}")]
    [RightsFilter(["Admin"])]
    public async Task<IActionResult> RemoveItemLine(int itemLineId)
    {
        if (itemLineId <= 0) return BadRequest("Can't remove item line with this id. ");

        bool IsRemoved = await _itemLineService.RemoveItemLine(itemLineId);
        if (!IsRemoved) return BadRequest("Couldn't remove item line with given id. ");
        return Ok("Item line removed. ");
    }
}