public class ShipmentServices
{
    private ShipmentAccess _shipmentAccess;

    private ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private InventoryAccess _inventoryAccess;
    public ShipmentServices(ShipmentAccess shipmentAccess, ShipmentItemMovementAccess shipmentItemMovementAccess, InventoryAccess inventoryAccess)
    {
        _shipmentAccess = shipmentAccess;
        _shipmentItemMovementAccess = shipmentItemMovementAccess;
        _inventoryAccess = inventoryAccess;
    }
    public async Task<List<Shipment>> GetShipments() => await _shipmentAccess.GetAll();

    public async Task<Shipment?> GetShipment(int shipmentId) => await _shipmentAccess.GetById(shipmentId);

    public async Task<List<ShipmentItemMovement>?> GetItemsInShipment(int shipmentId)
    {
        Shipment? shipment = await GetShipment(shipmentId);
        if (shipment is null) return [];
        return shipment.Items;
    }

    public async Task<bool> AddShipment(Shipment shipment)
    {
        if (shipment is null) return false;

        List<Shipment> shipments = await GetShipments();
        Shipment doubleShipment = shipments.FirstOrDefault(s => s.OrderId == shipment.OrderId && s.SourceId == shipment.SourceId && s.OrderDate == shipment.OrderDate && s.RequestDate == shipment.RequestDate && s.ShipmentDate == shipment.ShipmentDate && s.ShipmentType == shipment.ShipmentType && s.Notes == shipment.Notes && s.CarrierCode == shipment.CarrierCode && s.CarrierDescription == shipment.CarrierDescription && s.ServiceCode == shipment.ServiceCode && s.PaymentType == shipment.PaymentType && s.TransferMode == shipment.TransferMode && s.TotalPackageCount == shipment.TotalPackageCount && s.TotalPackageWeight == shipment.TotalPackageWeight && s.Items == shipment.Items)!;
        if (doubleShipment is not null) return false;
        return await _shipmentAccess.Add(shipment); ;
    }

    public async Task<bool> UpdateShipment(Shipment shipment)
    {
        if (shipment is null || shipment.Id == 0) return false;

        shipment.UpdatedAt = DateTime.Now;
        return await _shipmentAccess.Update(shipment); ;
    }

    private bool _updateItemsinInventory(Shipment shipment, Inventory inventory, int oldAmount, ShipmentItemMovement newItemMovement)
    {
        if (shipment!.ShipmentType == 'O')
        {
            if (shipment!.ShipmentStatus == "pending")
            {
                int changeamount = newItemMovement.Amount - oldAmount;
                inventory.TotalAllocated -= changeamount;
                _inventoryAccess.Update(inventory);
                return true;
            }
        }
        return false;
    }

    public async Task<bool> UpdateItemsinShipment(int shipmentId, List<ShipmentItemMovement> items)
    {
        try
        {
            List<ShipmentItemMovement?> shipmentItemMovements = await _shipmentItemMovementAccess.GetAllByOrderId(shipmentId);
            Shipment shipment = await _shipmentAccess.GetById(shipmentId);
            foreach (ShipmentItemMovement shipmentItemMovement in shipmentItemMovements)
            {
                ShipmentItemMovement changeInItem = items.First(item => item.Id == shipmentItemMovement!.ItemId);

                if (items.Select(item => item.ItemId).Contains(shipmentItemMovement!.ItemId))
                {
                    changeInItem.Id = shipmentItemMovement.Id;
                    await _shipmentItemMovementAccess.Update(changeInItem);
                }
                else
                {
                    await _shipmentItemMovementAccess.Remove(shipmentItemMovement.Id);
                }


                //update inventory based on order
                Inventory inventory = await _inventoryAccess.GetInventoryByItemId(shipmentItemMovement.ItemId);
                _updateItemsinInventory(shipment, inventory, shipmentItemMovement.Amount, changeInItem);
            }

            //check for new Items that were not in old
            foreach (ShipmentItemMovement shipmentItemMovementNew in items)
            {
                Inventory inventory = await _inventoryAccess.GetInventoryByItemId(shipmentItemMovementNew.ItemId);
                if (!shipmentItemMovements.Contains(shipmentItemMovementNew))
                {
                    await _shipmentItemMovementAccess.Add(shipmentItemMovementNew);

                    _updateItemsinInventory(shipment, inventory, 0, shipmentItemMovementNew);
                }
            }
            return true;
        }

        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> RemoveShipment(int shipmentId) => await _shipmentAccess.Remove(shipmentId);
}