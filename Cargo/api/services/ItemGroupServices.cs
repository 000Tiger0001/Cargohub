public class ItemGroupServices
{
    private readonly ItemGroupAccess _itemGroupAccess;
    private readonly ItemAccess _itemAccess;

    public ItemGroupServices(ItemGroupAccess itemGroupAccess, ItemAccess itemAccess)
    {
        _itemGroupAccess = itemGroupAccess;
        _itemAccess = itemAccess;
    }

    public async Task<List<ItemGroup>> GetItemGroups() => await _itemGroupAccess.GetAll();

    public async Task<ItemGroup?> GetItemGroup(int itemGroupId) => await _itemGroupAccess.GetById(itemGroupId);

    public async Task<bool> AddItemGroup(ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Name == "") return false;
        List<ItemGroup> itemGroups = await GetItemGroups();
        ItemGroup doubleItemGroup = itemGroups.Find(i => i.Name == itemGroup.Name)!;
        if (doubleItemGroup is not null) return false;
        return await _itemGroupAccess.Add(itemGroup);
    }

    public async Task<bool> UpdateItemGroup(ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Id == 0) return false;
        itemGroup.UpdatedAt = DateTime.Now;
        return await _itemGroupAccess.Update(itemGroup);
    }

    public async Task<bool> RemoveItemGroup(int itemGroupId)
    {
        List<Item> items = await _itemAccess.GetAll();
        items.ForEach(i => { if (itemGroupId == i.ItemGroupId) i.ItemGroupId = 0; });
        await _itemAccess.UpdateMany(items);
        bool IsRemoved = await _itemGroupAccess.Remove(itemGroupId);
        return IsRemoved;
    }
}