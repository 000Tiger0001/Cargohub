using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class TransferControllers : Controller
{
    private TransferServices TS;

    public TransferControllers(TransferServices ts)
    {
        TS = ts;
    }

    [HttpGet("get-transfers")]
    public async Task<IActionResult> GetTransfers() => Ok(await TS.GetTransfers());

    [HttpGet("get-transfer")]
    public async Task<IActionResult> GetTransfer([FromQuery] int transferId)
    {
        if (transferId == 0) return BadRequest("Can't proccess empty id. ");

        Transfer? transfer = await TS.GetTransfer(transferId);
        if (transfer is null) return BadRequest("Transfer with this id doesn't exist. ");
        return Ok(transfer);
    }

    [HttpGet("get-items-in-tranfer")]
    public async Task<IActionResult> GetItemsInTransfer([FromQuery] int transferId)
    {
        if (transferId == 0) return BadRequest("Can't proccess empty id. ");

        List<TransferItemMovement>? items = await TS.GetItemsInTransfer(transferId);
        if (items == default) return BadRequest("Transfer doesn't have any items. ");
        return Ok(items);
    }

    [HttpPost("add-transfer")]
    public async Task<IActionResult> AddTransfer([FromBody] Transfer transfer)
    {
        if (transfer is null || transfer.Reference == "" || (transfer.TransferFrom == 0 && transfer.TransferTo == 0) || transfer.TransferStatus == "") return BadRequest("Not enough given. ");

        bool IsAdded = await TS.AddTransfer(transfer);
        if (!IsAdded) return BadRequest("Transfer can't be added. ");
        return Ok("Transfer added. ");
    }

    [HttpPut("update-transfer")]
    public async Task<IActionResult> UpdateTransfer([FromBody] Transfer transfer)
    {
        if (transfer is null || transfer.Id == 0) return BadRequest("Not enough info given. ");

        bool IsUpdated = await TS.UpdateTransfer(transfer);
        if (!IsUpdated) return BadRequest("Transfer couldn't be updated. ");
        return Ok("Transfer updated. ");
    }

    [HttpDelete("remove-transfer")]
    public async Task<IActionResult> RemoveTransfer([FromQuery] int transferId)
    {
        if (transferId == 0) BadRequest("Can't proccess empty string. ");

        bool IsRemoved = await TS.RemoveTransfer(transferId);
        if (!IsRemoved) return BadRequest("Transfer doesn't exist. ");
        return Ok("Transfer removed. ");
    }
}