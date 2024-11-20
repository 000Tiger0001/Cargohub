using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ShipmentControllers : Controller
{
    ShipmentServices SS;

    public ShipmentControllers(ShipmentServices ss)
    {
        SS = ss;
    }

    [HttpGet("get-shipments")]
    public async Task<IActionResult> GetShipments() => Ok(await SS.GetShipments());

    [HttpGet("get-shipment")]
    public async Task<IActionResult> GetShipment([FromQuery] Guid shipmentId)
    {
        if (shipmentId == Guid.Empty) return BadRequest("Not enough info given. ");

        Shipment shipment = await SS.GetShipment(shipmentId);
        if (shipment is null) return BadRequest("Couldn't find shipment with given id. ");
        return Ok(shipment);
    }

    public async Task<IActionResult> GetItemsInShipment([FromQuery] Guid shipmentId)
    {
        if (shipmentId == Guid.Empty) return BadRequest("Not enough info given. ");

        Dictionary<Guid, int> items = await SS.GetItemsInShipment(shipmentId);
        if (items.Count == 0) return BadRequest("Couldn't find any items in shipment with given id. ");
        return Ok(items);
    }

    public async Task<IActionResult> AddShipment([FromBody] Shipment shipment)
    {
        if (shipment is null || shipment.CarrierCode == "" || shipment.CarrierDescription == "" || shipment.Items.Count == 0 || shipment.Notes == "" || shipment.OrderDate == DateTime.MinValue || shipment.OrderDate == DateTime.MaxValue || shipment.OrderIds.Count == 0 || shipment.PaymentType == "" || shipment.RequestDate == DateTime.MinValue || shipment.RequestDate == DateTime.MaxValue || shipment.ServiceCode == "" || shipment.ShipmentDate == DateTime.MinValue || shipment.ShipmentDate == DateTime.MaxValue || shipment.ShipmentStatus == "" || shipment.ShipmentType == default || shipment.SourceId == 0 || shipment.TotalPackageCount == 0.0 || shipment.TotalPackageWeight == 0.0 || shipment.TransferMode == "") return BadRequest("Not enough info given. ");

        shipment.Id = Guid.NewGuid();
        bool IsAdded = await SS.AddShipment(shipment);
        if (!IsAdded) return BadRequest("Couldn't add shipment. ");
        return Ok("Shipment added. ");
    }

    public async Task<IActionResult> UpdateShipment([FromBody] Shipment shipment)
    {
        if (shipment is null || shipment.Id == Guid.Empty) return BadRequest("Not enough info given. ");

        bool IsUpdated = await SS.UpdateShipment(shipment);
        if(!IsUpdated) return BadRequest("Couldn't update shipment. ");
        return Ok("Shipment updated. ");
    }

    public async Task<IActionResult> RemoveShipment([FromQuery] Guid shipmentId)
    {
        if (shipmentId == Guid.Empty) return BadRequest("Given id is empty. ");

        bool IsRemoved = await SS.RemoveShipment(shipmentId);
        if (!IsRemoved) return BadRequest("Couldn't remove shipment. ");
        return Ok("Shipment Removed. ");
    }
}