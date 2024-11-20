public class ItemGroupServices
{
    public async Task<List<ItemGroup>> GetItemGroups() => await AccessJson.ReadJson<ItemGroup>();

    public async Task<ItemGroup> GetItemGroup(Guid itemGroupId)
    {
        List<ItemGroup> itemGroups = await GetItemGroups();
        return itemGroups.FirstOrDefault(i => i.Id == itemGroupId)!;
    }

    public async Task<bool> AddItemGroup(ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Name == "") return false;
        List<ItemGroup> itemGroups = await GetItemGroups();
        ItemGroup doubleItemGroup = itemGroups.Find(i => i.Name == itemGroup.Name)!;
        if (doubleItemGroup is null) return false;
        await AccessJson.WriteJson(itemGroup);
        return true;
    }

    public async Task<bool> UpdateItemGroup(ItemGroup itemGroup)
    {
        if (itemGroup is null) return false;
        List<ItemGroup> itemGroups = await GetItemGroups();
        int itemGroupIndex = itemGroups.FindIndex(i => i.Id == itemGroup.Id);
        if (itemGroupIndex == -1) return false;
        itemGroup.UpdatedAt = DateTime.Now;
        itemGroups[itemGroupIndex] = itemGroup;
        AccessJson.WriteJsonList(itemGroups);
        return true;
    }

    public async Task<bool> RemoveItemGroup(Guid itemGroupId)
    {
        if (itemGroupId == Guid.Empty) return false;
        List<ItemGroup> itemGroups = await GetItemGroups();
        ItemGroup foundItemGroup = itemGroups.FirstOrDefault(i => i.Id == itemGroupId)!;
        if (foundItemGroup is null) return false;
        itemGroups.Remove(foundItemGroup);
        AccessJson.WriteJsonList(itemGroups);
        return true;
    }
}