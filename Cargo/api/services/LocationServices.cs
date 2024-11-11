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
}