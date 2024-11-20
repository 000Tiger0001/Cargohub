public class ItemTypeServices
{
    public async Task<List<ItemType>> GetItemTypes() => await AccessJson.ReadJson<ItemType>();

    public async Task<ItemType> GetItemType(Guid itemTypeId)
    {
        List<ItemType> itemTypes = await GetItemTypes();
        return itemTypes.FirstOrDefault(i => i.Id == itemTypeId)!;
    }

    public async Task<bool> AddItemType(ItemType itemType)
    {
        if (itemType is null || itemType.Name == "") return false;
        List<ItemType> itemTypes = await GetItemTypes();
        ItemType doubleItemType = itemTypes.Find(i => i.Name == itemType.Name)!;
        if (doubleItemType is null) return false;
        await AccessJson.WriteJson(itemType);
        return true;
    }

    public async Task<bool> UpdateItemType(ItemType itemType)
    {
        if (itemType is null) return false;
        List<ItemType> itemTypes = await GetItemTypes();
        int itemTypeIndex = itemTypes.FindIndex(i => i.Id == itemType.Id);
        if (itemTypeIndex == -1) return false;
        itemType.UpdatedAt = DateTime.Now;
        itemTypes[itemTypeIndex] = itemType;
        AccessJson.WriteJsonList(itemTypes);
        return true;
    }

    public async Task<bool> RemoveItemType(Guid itemTypeId)
    {
        if (itemTypeId == Guid.Empty) return false;
        List<ItemType> itemTypes = await GetItemTypes();
        ItemType foundItemType = itemTypes.FirstOrDefault(i => i.Id == itemTypeId)!;
        if (foundItemType is null) return false;
        itemTypes.Remove(foundItemType);
        AccessJson.WriteJsonList(itemTypes);
        return true;
    }
}