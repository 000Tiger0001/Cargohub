public class LocationServices
{
    private readonly LocationAccess _locationAccess;
    private readonly WarehouseAccess _warehouseAccess;

    public LocationServices(LocationAccess locationAcces, WarehouseAccess warehouseAccess)
    {
        _locationAccess = locationAcces;
        _warehouseAccess = warehouseAccess;
    }

    public async Task<List<Location>> GetLocations() => await _locationAccess.GetAll();

    public async Task<Location?> GetLocation(int locationId) => await _locationAccess.GetById(locationId)!;

    public async Task<List<Location>> GetLocationsInWarehouse(int warehouseId)
    {
        List<Location> locations = await GetLocations();
        return locations.FindAll(l => l.WarehouseId == warehouseId);
    }

    public async Task<bool> AddLocation(Location location)
    {
        List<Location> locations = await GetLocations();
        Location doubleLocation = locations.FirstOrDefault(l => l.Code == location.Code && l.Name == location.Name && l.WarehouseId == location.WarehouseId)!;
        if (doubleLocation is not null) return false;
        Warehouse? warehouse = await _warehouseAccess.GetById(location.WarehouseId)!;
        if (warehouse is null) return false;
        return await _locationAccess.Add(location);
    }

    public async Task<bool> UpdateLocation(Location location)
    {
        if (location is null || location.Id == 0) return false;
        location.UpdatedAt = DateTime.Now;
        return await _locationAccess.Update(location);
    }

    public async Task<bool> RemoveLocation(int locationId) => await _locationAccess.Remove(locationId);
}