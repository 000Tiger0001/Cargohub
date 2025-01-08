public class InventoryServices
{
    private readonly InventoryAccess _inventoryAccess;
    private readonly LocationAccess _locationAccess;
    private readonly ItemAccess _itemAccess;
    private readonly UserAccess _userAccess;
    private readonly LocationServices _locationServices;

    public InventoryServices(InventoryAccess inventoryAccess, LocationAccess locationAccess, ItemAccess itemAccess, UserAccess userAccess, LocationServices locationServices)
    {
        _inventoryAccess = inventoryAccess;
        _locationAccess = locationAccess;
        _itemAccess = itemAccess;
        _userAccess = userAccess;
        _locationAccess = locationAccess;
        _locationServices = locationServices;
    }

    public async Task<List<Inventory>> GetInventories() => await _inventoryAccess.GetAll();

    public async Task<Inventory?> GetInventory(int inventoryId) => await _inventoryAccess.GetById(inventoryId);

    public async Task<List<Inventory>> GetInventoriesforItem(int itemId)
    {
        List<Inventory> inventories = await GetInventories();
        return inventories.Where(i => i.ItemId == itemId).ToList();
    }

    public async Task<List<Inventory>> GetInventoriesforUser(int userId)
    {
        List<Location> locations = await _locationServices.GetLocationsOfUser(userId);
        List<int> locationIds = locations.Select(location => location.Id).ToList();
        List<Inventory> inventories = await GetInventories();
        return inventories.Where(inventory => inventory.Locations!.Any(location => locationIds.Contains(location))).ToList();
    }

    public async Task<Dictionary<string, int>> GetInventoryTotalsForItem(int itemId)
    {
        List<Inventory> inventories = await GetInventoriesforItem(itemId);
        Dictionary<string, int> result = new()
        {
            {"total_expected", 0},
            {"total_ordered", 0},
            {"total_allocated", 0},
            {"total_available", 0}
        };

        foreach (Inventory inventory in inventories)
        {
            result["total_expected"] += inventory.TotalExpected;
            result["total_ordered"] += inventory.TotalOrdered;
            result["total_allocated"] += inventory.TotalAllocated;
            result["total_available"] += inventory.TotalAvailable;
        }
        return result;
    }

    public async Task<bool> AddInventory(Inventory inventory)
    {
        List<Inventory> inventories = await GetInventories();
        List<Item> items = await _itemAccess.GetAll();
        if (inventories.FirstOrDefault(i => i.ItemId == inventory.ItemId || i.ItemReference == inventory.ItemReference) is not null || items.FirstOrDefault(i => i.Id == inventory.ItemId) is null) return false;
        List<Location> locations = await _locationAccess.GetAll();
        foreach (int locationId in inventory.Locations!) if (locations.FirstOrDefault(l => l.Id == locationId) is null) return false;
        return await _inventoryAccess.Add(inventory);
    }

    public async Task<bool> UpdateInventory(Inventory inventory)
    {
        if (inventory is null || inventory.Id <= 0) return false;
        inventory.UpdatedAt = DateTime.Now;
        return await _inventoryAccess.Update(inventory);
    }

    public async Task<bool> RemoveInventory(int inventoryId) => await _inventoryAccess.Remove(inventoryId);
}