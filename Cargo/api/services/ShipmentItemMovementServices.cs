public class ShipmentItemMovementServices
{
    private ShipmentItemMovementAccess _access;

    public ShipmentItemMovementServices(ShipmentItemMovementAccess shipmentItemMovementAccess)
    {
        _access = shipmentItemMovementAccess;
    }

    public async Task<List<ShipmentItemMovement>> GetShipmentItemMovements() => await _access.GetAll();

    public async Task<ShipmentItemMovement?> GetShipmentItemMovement(int shipmentItemMovementId) => await _access.GetById(shipmentItemMovementId);

    public async Task<bool> AddShipmentItemMovement(ShipmentItemMovement shipmentItemMovement)
    {
        List<ShipmentItemMovement> shipmentItemMovements = await GetShipmentItemMovements();
        ShipmentItemMovement doubleShipmentItemMovement = shipmentItemMovements.FirstOrDefault(s => s.Id == shipmentItemMovement.Id || (s.Amount == shipmentItemMovement.Amount && s.ItemId == shipmentItemMovement.ItemId))!;
        if (doubleShipmentItemMovement is not null) return false;
        return await _access.Add(shipmentItemMovement);
    }

    public async Task<bool> UpdateShipmentItemMovement(ShipmentItemMovement shipmentItemMovement)
    {
        if (shipmentItemMovement is null || shipmentItemMovement.Id == 0 || shipmentItemMovement.ItemId == 0 || shipmentItemMovement.Amount == 0) return false;
        return await _access.Update(shipmentItemMovement);
    }

    public async Task<bool> RemoveShipmentItemMovement(int shipmentItemMovementId) => await _access.Remove(shipmentItemMovementId);
}