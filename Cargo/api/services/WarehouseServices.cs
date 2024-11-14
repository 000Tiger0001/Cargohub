public class WarehouseServices
{
    public async Task<List<Warehouse>> GetWarehouses()
    {
        List<Warehouse> warehouses = await AccessJson.ReadJson<Warehouse>();
        return warehouses;
    }

    public async Task<Warehouse> GetWarehouse(Guid warehouseId)
    {
        List<Warehouse> warehouses = await GetWarehouses();
        return warehouses.FirstOrDefault(w => w.Id == warehouseId)!;
    }

    public async Task<bool> AddWarehouse(Warehouse warehouse)
    {
        List<Warehouse> warehouses = await GetWarehouses();
        warehouse.Id = Guid.NewGuid();
        Warehouse doubleWarehouse = warehouses.FirstOrDefault(w => w.Code == warehouse.Code && w.Name == warehouse.Name && w.Address == warehouse.Address && w.Zip == warehouse.Zip && w.City == warehouse.City && w.Province == warehouse.Province && w.Country == warehouse.Country)!;
        if (doubleWarehouse is not null) return false;
        await AccessJson.WriteJson(warehouse);
        return true;
    }
    public async Task<bool> UpdateWarehouse(Warehouse warehouse)
    {
        List<Warehouse> warehouses = await GetWarehouses();
        int warehouseIndex = warehouses.FindIndex(w => w.Id == warehouse.Id);
        if (warehouseIndex == -1) return false;
        warehouse.UpdatedAt = DateTime.Now;
        warehouses[warehouseIndex] = warehouse;
        AccessJson.WriteJsonList(warehouses);
        return true;
    }

    public async Task<bool> RemoveWarehouse(Guid warehouseId)
    {
        List<Warehouse> warehouses = await GetWarehouses();
        Warehouse warehouseToRemove = warehouses.FirstOrDefault(w => w.Id == warehouseId)!;
        if (warehouseToRemove is null) return false;
        warehouses.Remove(warehouseToRemove);
        AccessJson.WriteJsonList(warehouses);
        return true;
    }
}