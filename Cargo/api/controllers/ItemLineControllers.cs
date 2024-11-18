using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ItemLineControllers : Controller
{
    ItemLineServices ILS;

    public ItemLineControllers(ItemLineServices ils)
    {
        ILS = ils;
    }

    [HttpGet("get-item-lines")]
    public async Task<IActionResult> GetItemLines() => Ok(await ILS.GetItemLines());

    [HttpGet("get-item-line")]
    public async Task<IActionResult> GetItemLine([FromQuery] Guid itemLineId)
    {
        if (itemLineId == Guid.Empty) return BadRequest("You can't use an empty string. ");
        ItemLine itemLine = await ILS.GetItemLine(itemLineId);
        if (itemLine is null) return BadRequest("Item line not found. ");
        return Ok(itemLine);
    }

    [HttpPost("add-item-line")]
    public async Task<IActionResult> AddItemLine([FromBody] ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Name == "") return BadRequest("Not enough info. ");

        bool IsAdded = await ILS.AddItemLine(itemLine);
        if (!IsAdded) return BadRequest("Can't add item line. ");
        return Ok("Item line added. ");
    }

    [HttpPut("update-item-line")]
    public async Task<IActionResult> UpdateItemLine([FromBody] ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Id == Guid.Empty) return BadRequest("Not enough info. ");

        bool IsUpdated = await ILS.UpdateItemLine(itemLine);
        if (!IsUpdated) return BadRequest("Item line can't be updated. ");
        return Ok("Item line updated. ");
    }

    [HttpDelete("remove-item-line")]
    public async Task<IActionResult> RemoveItemLine([FromQuery] Guid itemLineId)
    {
        if (itemLineId == Guid.Empty) return BadRequest("Can't remove item line with empty id. ");

        bool IsRemoved = await ILS.RemoveItemLine(itemLineId);
        if (!IsRemoved) return BadRequest("Couldn't remove item line with given id. ");
        return Ok("Item line removed. ");
    }
}