public class LocationServices
{
    public async Task<List<Location>> GetLocations()
    {
        List<Location> locations = await AccessJson.ReadJson<Location>();
        return locations;
    }

    public async Task<Location> GetLocation(Guid locationId)
    {
        List<Location> locations = await AccessJson.ReadJson<Location>();
        Location location = locations.FirstOrDefault(l => l.Id == locationId)!;
        return location;
    }

    public async Task<List<Location>> GetLocationsInWarehouse(Guid warehouseId)
    {
        List<Location> locations = await AccessJson.ReadJson<Location>();
        List<Location> locationsInWarehouse = locations.FindAll(l => l.WarehouseId == warehouseId);
        return locationsInWarehouse;
    }

    public async Task<bool> AddLocation(Location location)
    {
        List<Location> locations = await AccessJson.ReadJson<Location>();
        Location doubleLocation = locations.FirstOrDefault(l => l.Code == location.Code && l.Name == location.Name && l.WarehouseId == location.WarehouseId)!;
        if (doubleLocation is not null) return false;
        AccessJson.WriteJson(location);
        return true;
    }

    public async Task<bool> UpdateLocation(Location location)
    {
        List<Location> locations = await AccessJson.ReadJson<Location>();
        int foundLocationIndex = locations.FindIndex(l => l.Id == location.Id);
        if (foundLocationIndex == -1) return false;
        location.UpdatedAt = DateTime.Now;
        locations[foundLocationIndex] = location;
        AccessJson.WriteJsonList(locations);
        return true;
    }

    public async Task<bool> RemoveLocation(Guid locationId)
    {
        List<Location> locations = await AccessJson.ReadJson<Location>();
        Location foundLocation = locations.FirstOrDefault(l => l.Id == locationId)!;
        if (foundLocation is null) return false;
        locations.Remove(foundLocation);
        AccessJson.WriteJsonList(locations);
        return true;
    }
}