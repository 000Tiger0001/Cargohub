public class WarehouseServices
{
    WarehouseAccess _warehouseAccess;

    public WarehouseServices(WarehouseAccess warehouseAccess)
    {
        _warehouseAccess = warehouseAccess;
    } 
    public async Task<List<Warehouse>> GetWarehouses() => await _warehouseAccess.GetAll();

    public async Task<Warehouse?> GetWarehouse(int warehouseId) => await _warehouseAccess.GetById(warehouseId)!;

    public async Task<bool> AddWarehouse(Warehouse warehouse)
    {
        List<Warehouse> warehouses = await GetWarehouses();

        Warehouse doubleWarehouse = warehouses.FirstOrDefault(w => w.Code == warehouse.Code && w.Name == warehouse.Name && w.Address == warehouse.Address && w.Zip == warehouse.Zip && w.City == warehouse.City && w.Province == warehouse.Province && w.Country == warehouse.Country)!;
        if (doubleWarehouse is not null) return false;

        bool IsAdded = await _warehouseAccess.Add(warehouse);
        return IsAdded;
    }
    public async Task<bool> UpdateWarehouse(Warehouse warehouse)
    {
        if (warehouse is null) return false;

        warehouse.UpdatedAt = DateTime.Now;
        bool IsUpdated = await _warehouseAccess.Update(warehouse);
        return IsUpdated;
    }

    public async Task<bool> RemoveWarehouse(int warehouseId) => await _warehouseAccess.Delete(warehouseId);
}