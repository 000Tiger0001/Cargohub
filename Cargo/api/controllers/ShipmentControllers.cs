using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ShipmentControllers : Controller
{
    private ShipmentServices _shipmentService;

    public ShipmentControllers(ShipmentAccess shipmentAccess)
    {
        _shipmentService = new(shipmentAccess, false);
    }

    [HttpGet("get-shipments")]
    public async Task<IActionResult> GetShipments() => Ok(await _shipmentService.GetShipments());

    [HttpGet("get-shipment")]
    public async Task<IActionResult> GetShipment([FromQuery] int shipmentId)
    {
        if (shipmentId == 0) return BadRequest("Not enough info given. ");

        Shipment? shipment = await _shipmentService.GetShipment(shipmentId);
        if (shipment is null) return BadRequest("Couldn't find shipment with given id. ");
        return Ok(shipment);
    }

    public async Task<IActionResult> GetItemsInShipment([FromQuery] int shipmentId)
    {
        if (shipmentId == 0) return BadRequest("Not enough info given. ");

        List<ShipmentItemMovement>? items = await _shipmentService.GetItemsInShipment(shipmentId);
        if (items == default || items.Count == 0) return BadRequest("Couldn't find any items in shipment with given id. ");
        return Ok(items);
    }

    public async Task<IActionResult> AddShipment([FromBody] Shipment shipment)
    {
        if (shipment is null || shipment.CarrierCode == "" || shipment.CarrierDescription == "" || shipment.Items == default ||shipment.Items.Count == 0 || shipment.Notes == "" || shipment.OrderDate == DateTime.MinValue || shipment.OrderDate == DateTime.MaxValue || shipment.OrderId == 0 || shipment.PaymentType == "" || shipment.RequestDate == DateTime.MinValue || shipment.RequestDate == DateTime.MaxValue || shipment.ServiceCode == "" || shipment.ShipmentDate == DateTime.MinValue || shipment.ShipmentDate == DateTime.MaxValue || shipment.ShipmentStatus == "" || shipment.ShipmentType == default || shipment.SourceId == 0 || shipment.TotalPackageCount == 0.0 || shipment.TotalPackageWeight == 0.0 || shipment.TransferMode == "") return BadRequest("Not enough info given. ");

        bool IsAdded = await _shipmentService.AddShipment(shipment);
        if (!IsAdded) return BadRequest("Couldn't add shipment. ");
        return Ok("Shipment added. ");
    }

    public async Task<IActionResult> UpdateShipment([FromBody] Shipment shipment)
    {
        if (shipment is null || shipment.Id == 0) return BadRequest("Not enough info given. ");

        bool IsUpdated = await _shipmentService.UpdateShipment(shipment);
        if(!IsUpdated) return BadRequest("Couldn't update shipment. ");
        return Ok("Shipment updated. ");
    }

    public async Task<IActionResult> RemoveShipment([FromQuery] int shipmentId)
    {
        if (shipmentId == 0) return BadRequest("Given id is empty. ");

        bool IsRemoved = await _shipmentService.RemoveShipment(shipmentId);
        if (!IsRemoved) return BadRequest("Couldn't remove shipment. ");
        return Ok("Shipment Removed. ");
    }
}