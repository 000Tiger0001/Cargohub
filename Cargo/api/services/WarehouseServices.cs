public class WarehouseServices
{
    private readonly WarehouseAccess _warehouseAccess;
    private readonly LocationAccess _locationAccess;

    public WarehouseServices(WarehouseAccess warehouseAccess, LocationAccess locationAccess)
    {
        _warehouseAccess = warehouseAccess;
        _locationAccess = locationAccess;
    } 
    public async Task<List<Warehouse>> GetWarehouses() => await _warehouseAccess.GetAll();

    public async Task<Warehouse?> GetWarehouse(int warehouseId) => await _warehouseAccess.GetById(warehouseId)!;

    public async Task<bool> AddWarehouse(Warehouse warehouse)
    {
        List<Warehouse> warehouses = await GetWarehouses();

        Warehouse doubleWarehouse = warehouses.FirstOrDefault(w => w.Code == warehouse.Code && w.Name == warehouse.Name && w.Address == warehouse.Address && w.Zip == warehouse.Zip && w.City == warehouse.City && w.Province == warehouse.Province && w.Country == warehouse.Country)!;
        if (doubleWarehouse is not null) return false;
        return await _warehouseAccess.Add(warehouse);
    }
    public async Task<bool> UpdateWarehouse(Warehouse warehouse)
    {
        if (warehouse is null || warehouse.Id == 0) return false;
        warehouse.UpdatedAt = DateTime.Now;
        return await _warehouseAccess.Update(warehouse);
    }

    public async Task<bool> RemoveWarehouse(int warehouseId)
    {
        List<Location> locations = await _locationAccess.GetAll();
        locations.ForEach(l => { if (l.WarehouseId == warehouseId) l.WarehouseId = 0; });
        await _locationAccess.UpdateMany(locations);
        return await _warehouseAccess.Remove(warehouseId);
    }
}