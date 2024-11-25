public class LocationServices
{
    private LocationAccess _locationAccess;
    private bool _debug;
    private List<Location> _testLocations;

    public LocationServices(LocationAccess locationAcces, bool debug)
    {
        _locationAccess = locationAcces;
        _debug = debug;
        _testLocations = [];
    }

    public async Task<List<Location>> GetLocations()
    {
        if (!_debug) return await _locationAccess.GetAll();
        return _testLocations;
    }

    public async Task<Location?> GetLocation(int locationId)
    {
        if (!_debug) return await _locationAccess.GetById(locationId)!;
        return _testLocations.FirstOrDefault(l => l.Id == locationId);
    }

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
        if (!_debug) return await _locationAccess.Add(location);
        _testLocations.Add(location);
        return true;
    }

    public async Task<bool> UpdateLocation(Location location)
    {
        if (location is null || location.Id == 0) return false;
        location.UpdatedAt = DateTime.Now;
        if (!_debug) return await _locationAccess.Update(location);
        int foundLocationIndex = _testLocations.FindIndex(l => l.Id == location.Id);
        if (foundLocationIndex == -1) return false;
        _testLocations[foundLocationIndex] = location;
        return true;
    }

    public async Task<bool> RemoveLocation(int locationId)
    {
        if (!_debug) return await _locationAccess.Remove(locationId);
        return _testLocations.Remove(_testLocations.FirstOrDefault(l => l.Id == locationId)!);
    }
}