public class LocationServices
{
    private LocationAccess _locationAcces;
    public LocationServices(LocationAccess locationAcces)
    {
        _locationAcces = locationAcces;
    }
    public async Task<List<Location>> GetLocations() => await _locationAcces.GetAll();

    public async Task<Location?> GetLocation(int locationId) => await _locationAcces.GetById(locationId)!;

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
        await _locationAcces.Add(location);
        return true;
    }

    public async Task<bool> UpdateLocation(Location location)
    {
        location.UpdatedAt = DateTime.Now;
        bool IsUpdated = await _locationAcces.Update(location);
        return IsUpdated;
    }

    public async Task<bool> RemoveLocation(int locationId) => await _locationAcces.Delete(locationId);
}