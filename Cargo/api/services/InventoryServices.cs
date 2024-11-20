public class InventoryServices
{
    public async Task<List<Inventory>> GetInventories() => await AccessJson.ReadJson<Inventory>();

    public async Task<Inventory> GetInventory(Guid inventoryId)
    {
        List<Inventory> inventories = await GetInventories();
        return inventories.FirstOrDefault(i => i.Id == inventoryId)!; ;
    }

    public async Task<List<Inventory>> GetInventoriesforItem(Guid itemId)
    {
        List<Inventory> inventories = await GetInventories();
        return inventories.Where(i => i.ItemId == itemId).ToList();
    }

    public async Task<Dictionary<string, int>> GetInventoryTotalsForItem(Guid itemId)
    {
        List<Inventory> inventories = await GetInventories();
        Dictionary<string, int> result = new()
        {
            {"total_expected", 0},
            {"total_ordered", 0},
            {"total_allocated", 0},
            {"total_available", 0}
        };

        foreach (Inventory inventory in inventories)
        {
            if (inventory.ItemId == itemId)
            {
                result["total_expected"] += inventory.TotalExpected;
                result["total_ordered"] += inventory.TotalOrdered;
                result["total_allocated"] += inventory.TotalAllocated;
                result["total_available"] += inventory.TotalAvailable;
            }
        }

        return result;
    }

    public async Task<bool> AddInventory(Inventory inventory)
    {
        List<Inventory> inventories = await GetInventories();
        inventory.Id = Guid.NewGuid();
        Inventory doubleInventory = inventories.FirstOrDefault(i => i.ItemId == inventory.ItemId || i.ItemReference == inventory.ItemReference)!;
        if (doubleInventory is not null) return false;
        await AccessJson.WriteJson(inventory);
        return true;
    }

    public async Task<bool> UpdateInventory(Inventory inventory)
    {
        List<Inventory> inventories = await GetInventories();
        int foundInventoryIndex = inventories.FindIndex(i => i.Id == inventory.Id);
        if (foundInventoryIndex == -1) return false;
        inventory.UpdatedAt = DateTime.Now;
        inventories[foundInventoryIndex] = inventory;
        AccessJson.WriteJsonList(inventories);
        return true;
    }

    public async Task<bool> RemoveInventory(Guid inventoryId)
    {
        if (inventoryId == Guid.Empty) return false;
        List<Inventory> inventories = await GetInventories();
        Inventory foundInventory = inventories.FirstOrDefault(i => i.Id == inventoryId)!;
        if (foundInventory is null) return false;
        inventories.Remove(foundInventory);
        AccessJson.WriteJsonList(inventories);
        return true;
    }
}