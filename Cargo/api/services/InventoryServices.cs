public class InventoryServices
{
    private InventoryAccess _inventoryAccess;
    private bool _debug;
    public List<Inventory> testInventories;

    public InventoryServices(InventoryAccess inventoryAccess, bool debug)
    {
        _inventoryAccess = inventoryAccess;
        _debug = debug;
        testInventories = [];
    }
    public async Task<List<Inventory>> GetInventories() => _debug ? testInventories : await _inventoryAccess.GetAll();

    public async Task<Inventory?> GetInventory(int inventoryId) => _debug ? testInventories.FirstOrDefault(i => i.Id == inventoryId) : await _inventoryAccess.GetById(inventoryId);

    public async Task<List<Inventory>> GetInventoriesforItem(int itemId)
    {
        List<Inventory> inventories = await GetInventories();
        return inventories.Where(i => i.ItemId == itemId).ToList();
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
        Inventory doubleInventory = inventories.FirstOrDefault(i => i.ItemId == inventory.ItemId || i.ItemReference == inventory.ItemReference)!;
        if (doubleInventory is not null) return false;
        if (!_debug) return await _inventoryAccess.Add(inventory);
        testInventories.Add(inventory);
        return true;
    }

    public async Task<bool> UpdateInventory(Inventory inventory)
    {
        if (inventory is null || inventory.Id == 0) return false;
        inventory.UpdatedAt = DateTime.Now;
        if (!_debug) return await _inventoryAccess.Update(inventory);
        int foundInventoryIndex = testInventories.FindIndex(i => i.Id == inventory.Id);
        if (foundInventoryIndex == -1) return false;
        testInventories[foundInventoryIndex] = inventory;
        return true;
    }

    public async Task<bool> RemoveInventory(int inventoryId) => _debug ? testInventories.Remove(testInventories.FirstOrDefault(i => i.Id == inventoryId)!) : await _inventoryAccess.Remove(inventoryId);
}