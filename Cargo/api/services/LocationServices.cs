public class LocationServices
{
    public async Task<List<Location>> GetLocations() => await AccessJson.ReadJson<Location>();

    public async Task<Location> GetLocation(Guid locationId)
    {
        List<Location> locations = await GetLocations();
        return locations.FirstOrDefault(l => l.Id == locationId)!;
    }

    public async Task<List<Location>> GetLocationsInWarehouse(Guid warehouseId)
    {
        List<Location> locations = await GetLocations();
        return locations.FindAll(l => l.WarehouseId == warehouseId);
    }

    public async Task<bool> AddLocation(Location location)
    {
        List<Location> locations = await GetLocations();
        Location doubleLocation = locations.FirstOrDefault(l => l.Code == location.Code && l.Name == location.Name && l.WarehouseId == location.WarehouseId)!;
        if (doubleLocation is not null) return false;
        await AccessJson.WriteJson(location);
        return true;
    }

    public async Task<bool> UpdateLocation(Location location)
    {
        List<Location> locations = await GetLocations();
        int foundLocationIndex = locations.FindIndex(l => l.Id == location.Id);
        if (foundLocationIndex == -1) return false;
        location.UpdatedAt = DateTime.Now;
        locations[foundLocationIndex] = location;
        AccessJson.WriteJsonList(locations);
        return true;
    }

    public async Task<bool> RemoveLocation(Guid locationId)
    {
        List<Location> locations = await GetLocations();
        Location foundLocation = locations.FirstOrDefault(l => l.Id == locationId)!;
        if (foundLocation is null) return false;
        locations.Remove(foundLocation);
        AccessJson.WriteJsonList(locations);
        return true;
    }
}