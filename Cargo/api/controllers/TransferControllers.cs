using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class TransferControllers : Controller
{
    private TransferServices _transferServices;

    public TransferControllers(TransferServices transferServices)
    {
        _transferServices = transferServices;
    }

    [HttpGet("transfers")]
    public async Task<IActionResult> GetTransfers() => Ok(await _transferServices.GetTransfers());

    [HttpGet("transfer/{transferId}")]
    public async Task<IActionResult> GetTransfer(int transferId)
    {
        if (transferId == 0) return BadRequest("Can't proccess empty id. ");

        Transfer? transfer = await _transferServices.GetTransfer(transferId);
        if (transfer is null) return BadRequest("Transfer with this id doesn't exist. ");
        return Ok(transfer);
    }

    [HttpGet("transfer/{transferId}/items")]
    public async Task<IActionResult> GetItemsInTransfer(int transferId)
    {
        if (transferId == 0) return BadRequest("Can't proccess empty id. ");

        List<TransferItemMovement>? items = await _transferServices.GetItemsInTransfer(transferId);
        if (items == default) return BadRequest("Transfer doesn't have any items. ");
        return Ok(items);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> AddTransfer([FromBody] Transfer transfer)
    {
        if (transfer is null || transfer.Reference == "" || (transfer.TransferFrom == 0 && transfer.TransferTo == 0) || transfer.TransferStatus == "") return BadRequest("Not enough given. ");

        bool IsAdded = await _transferServices.AddTransfer(transfer);
        if (!IsAdded) return BadRequest("Transfer can't be added. ");
        return Ok("Transfer added. ");
    }

    [HttpPut("transfer")]
    public async Task<IActionResult> UpdateTransfer([FromBody] Transfer transfer)
    {
        if (transfer is null || transfer.Id == 0) return BadRequest("Not enough info given. ");

        bool IsUpdated = await _transferServices.UpdateTransfer(transfer);
        if (!IsUpdated) return BadRequest("Transfer couldn't be updated. ");
        return Ok("Transfer updated. ");
    }

    [HttpDelete("transfer/{transferId}")]
    public async Task<IActionResult> RemoveTransfer(int transferId)
    {
        if (transferId == 0) BadRequest("Can't proccess empty string. ");

        bool IsRemoved = await _transferServices.RemoveTransfer(transferId);
        if (!IsRemoved) return BadRequest("Transfer doesn't exist. ");
        return Ok("Transfer removed. ");
    }
}