public class ShipmentServices
{
    public async Task<List<Shipment>> GetShipments()
    {
        List<Shipment> shipments = await AccessJson.ReadJson<Shipment>();
        return shipments;
    }

    public async Task<Shipment> GetShipment(Guid shipmentId)
    {
        List<Shipment> shipments = await AccessJson.ReadJson<Shipment>();
        Shipment foundShipment = shipments.FirstOrDefault(s => s.Id == shipmentId);
        return foundShipment;
    }

    public async Task<Dictionary<Guid, int>> GetItemsInShipment(Guid shipmentId)
    {
        List<Shipment> shipments = await AccessJson.ReadJson<Shipment>();
        Shipment foundShipment = shipments.FirstOrDefault(s => s.Id == shipmentId);
        if (foundShipment is null) return [];
        return foundShipment.Items;
    }

    public async Task<bool> AddShipment(Shipment shipment)
    {
        List<Shipment> shipments = await AccessJson.ReadJson<Shipment>();
        shipment.Id = Guid.NewGuid();
        Shipment doubleShipment = shipments.FirstOrDefault(s => s.OrderId == shipment.OrderId && s.SourceId == shipment.SourceId && s.OrderDate == shipment.OrderDate && s.RequestDate == shipment.RequestDate && s.ShipmentDate == shipment.ShipmentDate && s.ShipmentType == shipment.ShipmentType && s.Notes == shipment.Notes && s.CarrierCode == shipment.CarrierCode && s.CarrierDescription == shipment.CarrierDescription && s.ServiceCode == shipment.ServiceCode && s.PaymentType == shipment.PaymentType && s.TransferMode == shipment.TransferMode && s.TotalPackageCount == shipment.TotalPackageCount && s.TotalPackageWeight == shipment.TotalPackageWeight && s.Items == shipment.Items);
        if (doubleShipment is not null) return false;
        await AccessJson.WriteJson(shipment);
        return true;
    }

    public async Task<bool> UpdateShipment(Shipment shipment)
    {
        List<Shipment> shipments = await AccessJson.ReadJson<Shipment>();
        int shipmentIndex = shipment.FindIndex(s => s.Id == shipment.Id);
        if (shipmentIndex == -1) return false;
        shipment.UpdatedAt = DateTime.Now;
        shipments[shipmentIndex] = shipment;
        AccessJson.WriteJsonList(shipments);
        return true;
    }

    /*public async Task<bool> UpdateItemsInShipment(Guid shipmentId, List<Item> items)
    {
        InventoryServices IS = new();
        if (shipmentId == Guid.Empty) return false;
        Shipment foundShipment = GetShipment(shipmentId);
        if (foundShipment is null) return false;
        Dictionary<Guid, int> currentItemsInShipment = foundShipment.Items;
        foreach (var x in currentItemsInShipment)
        {
            bool found = false;
            foreach (Item y in items)
            {
                if (x.Key == y.Id)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                List<Inventory> inventories = IS.GetInventoriesforItem(x.Key);
                int maxOrdered = -1;
                Inventory maxInventory;
                foreach (Inventory z in inventories)
                {
                    if (z.TotalOrdered > maxOrdered)
                    {
                        maxOrdered = z.TotalOrdered;
                        maxInventory = z;
                    }
                }
                maxInventory.TotalOrdered -= x.Value
                maxInventory.TotalExpected = y
            }
        }
    }*/

    public async Task<bool> RemoveShipment(Guid shipmentId)
    {
        List<Shipment> shipments = await AccessJson.ReadJson<Shipment>();
        Shipment shipmentToRemove = shipments.FirstOrDefault(s => s.Id == shipmentId);
        if (shipmentToRemove is null) return false;
        shipments.Remove(shipmentToRemove);
        AccessJson.WriteJsonList(shipments);
        return true;
    }
}