public class ShipmentServices
{
    private readonly ShipmentAccess _shipmentAccess;
    private readonly ItemAccess _itemAccess;
    private readonly OrderAccess _orderAccess;

    public ShipmentServices(ShipmentAccess shipmentAccess, ItemAccess itemAccess, OrderAccess orderAccess)
    {
        _shipmentAccess = shipmentAccess;
        _itemAccess = itemAccess;
        _orderAccess = orderAccess;
    }

    public async Task<List<Shipment>> GetShipments() => await _shipmentAccess.GetAll();

    public async Task<Shipment?> GetShipment(int shipmentId) => await _shipmentAccess.GetById(shipmentId);

    public async Task<List<ShipmentItemMovement>> GetItemsInShipment(int shipmentId)
    {
        Shipment? shipment = await GetShipment(shipmentId);
        if (shipment is null) return [];
        return shipment.Items!;
    }

    public async Task<bool> AddShipment(Shipment shipment)
    {
        if (shipment is null) return false;
        List<Shipment> shipments = await GetShipments();
        Shipment doubleShipment = shipments.FirstOrDefault(s => s.OrderId == shipment.OrderId && s.SourceId == shipment.SourceId && s.OrderDate == shipment.OrderDate && s.RequestDate == shipment.RequestDate && s.ShipmentDate == shipment.ShipmentDate && s.ShipmentType == shipment.ShipmentType && s.Notes == shipment.Notes && s.CarrierCode == shipment.CarrierCode && s.CarrierDescription == shipment.CarrierDescription && s.ServiceCode == shipment.ServiceCode && s.PaymentType == shipment.PaymentType && s.TransferMode == shipment.TransferMode && s.TotalPackageCount == shipment.TotalPackageCount && s.TotalPackageWeight == shipment.TotalPackageWeight && s.Items == shipment.Items)!;
        List<Item> items = await _itemAccess.GetAll();
        List<Order> orders = await _orderAccess.GetAll();
        Order foundOrder = orders.FirstOrDefault(o => o.ShipmentId == shipment.Id)!;
        if (doubleShipment is not null || foundOrder is null) return false;
        foreach (ShipmentItemMovement shipmentItemMovement in shipment.Items!) if (items.FirstOrDefault(i => i.Id == shipmentItemMovement.ItemId) is null) return false;
        return await _shipmentAccess.Add(shipment);
    }

    public async Task<bool> UpdateShipment(Shipment shipment)
    {
        if (shipment is null || shipment.Id <= 0) return false;
        shipment.UpdatedAt = DateTime.Now;
        return await _shipmentAccess.Update(shipment);
    }

    public async Task<bool> RemoveShipment(int shipmentId) => await _shipmentAccess.Remove(shipmentId);
}