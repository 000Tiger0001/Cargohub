public class ShipmentServices
{
    private readonly ShipmentAccess _shipmentAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private readonly InventoryAccess _inventoryAccess;
    private readonly ItemAccess _itemAccess;
    private readonly OrderAccess _orderAccess;

    public ShipmentServices(ShipmentAccess shipmentAccess, ShipmentItemMovementAccess shipmentItemMovementAccess, InventoryAccess inventoryAccess, ItemAccess itemAccess, OrderAccess orderAccess)
    {
        _shipmentAccess = shipmentAccess;
        _shipmentItemMovementAccess = shipmentItemMovementAccess;
        _inventoryAccess = inventoryAccess;
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
        if (shipment.ShipmentStatus != "Transit" && shipment.ShipmentStatus != "Pending" && shipment.ShipmentStatus != "Delivered") return false;
        List<Shipment> shipments = await GetShipments();
        Shipment doubleShipment = shipments.FirstOrDefault(s => s.OrderId == shipment.OrderId && s.SourceId == shipment.SourceId && s.OrderDate == shipment.OrderDate && s.RequestDate == shipment.RequestDate && s.ShipmentDate == shipment.ShipmentDate && s.ShipmentType == shipment.ShipmentType && s.Notes == shipment.Notes && s.CarrierCode == shipment.CarrierCode && s.CarrierDescription == shipment.CarrierDescription && s.ServiceCode == shipment.ServiceCode && s.PaymentType == shipment.PaymentType && s.TransferMode == shipment.TransferMode && s.TotalPackageCount == shipment.TotalPackageCount && s.TotalPackageWeight == shipment.TotalPackageWeight && s.Items == shipment.Items)!;
        List<Item> items = await _itemAccess.GetAll();
        List<Order> orders = await _orderAccess.GetAll();
        Order foundOrder = orders.FirstOrDefault(o => o.OrderId == shipment.OrderId)!;
        if (doubleShipment is not null || foundOrder is null) return false;
        foreach (ShipmentItemMovement shipmentItemMovement in shipment.Items!) if (items.FirstOrDefault(i => i.Id == shipmentItemMovement.ItemId) is null) return false;
        return await _shipmentAccess.Add(shipment);
    }

    public async Task<bool> UpdateShipment(Shipment shipment)
    {
        if (shipment is null || shipment.Id <= 0 || shipment.ShipmentStatus == "Delivered") return false;
        if (shipment.ShipmentStatus != "Transit" && shipment.ShipmentStatus != "Pending" && shipment.ShipmentStatus != "Delivered") return false;
        shipment.UpdatedAt = DateTime.Now;
        return await _shipmentAccess.Update(shipment);
    }

    private async Task<bool> _updateItemsinInventory(Shipment shipment, Inventory inventory, int oldAmount, ShipmentItemMovement newItemMovement)
    {
        if (shipment!.ShipmentType == 'O')
        {
            if (shipment!.ShipmentStatus == "Pending")
            {
                int changeamount = newItemMovement.Amount - oldAmount;
                inventory.TotalAllocated -= changeamount;
                await _inventoryAccess.Update(inventory);
                return true;
            }
        }

        if (shipment!.ShipmentType == 'I')
        {
            if (shipment!.ShipmentStatus == "Pending")
            {
                int changeamount = newItemMovement.Amount - oldAmount;
                inventory.TotalExpected += changeamount;
                await _inventoryAccess.Update(inventory);
                return true;
            }
        }
        return false;
    }

    public async Task<bool> UpdateItemsinShipment(int shipmentId, List<ShipmentItemMovement> items)
    {
        try
        {
            List<ShipmentItemMovement> shipmentItemMovements = await _shipmentItemMovementAccess.GetAllByOrderId(shipmentId);
            Shipment? shipment = await _shipmentAccess.GetById(shipmentId);


            //check for new Items that were not in old
            foreach (ShipmentItemMovement shipmentItemMovementNew in items)
            {
                Inventory? inventory = await _inventoryAccess.GetInventoryByItemId(shipmentItemMovementNew.ItemId);
                if (!shipmentItemMovements.Any(sim => sim.ItemId == shipmentItemMovementNew.ItemId))
                {
                    shipmentItemMovementNew.Shipment_Id = shipmentId;
                    await _shipmentItemMovementAccess.Add(shipmentItemMovementNew);
                    await _updateItemsinInventory(shipment!, inventory!, 0, shipmentItemMovementNew);
                }
            }
            foreach (ShipmentItemMovement? shipmentItemMovement in shipmentItemMovements)
            {
                ShipmentItemMovement changeInItem = items.FirstOrDefault(item => item.ItemId == shipmentItemMovement!.ItemId)!;
                Inventory? inventory = await _inventoryAccess.GetInventoryByItemId(shipmentItemMovement.ItemId);
                if (changeInItem is null)
                {
                    await _shipmentItemMovementAccess.Remove(shipmentItemMovement.Id);
                    await _updateItemsinInventory(shipment!, inventory!, shipmentItemMovement.Amount, new() { Amount = 0 });
                    continue;
                }
                changeInItem.Id = shipmentItemMovement.Id;
                changeInItem.Shipment_Id = shipmentId;
                if (items.Select(item => item.ItemId).Contains(shipmentItemMovement!.ItemId)) await _shipmentItemMovementAccess.Update(changeInItem);

                //update inventory based on order\
                await _updateItemsinInventory(shipment!, inventory!, shipmentItemMovement.Amount, changeInItem);
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> RemoveShipment(int shipmentId) => await _shipmentAccess.Remove(shipmentId);
}