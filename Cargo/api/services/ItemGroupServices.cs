public class ItemGroupServices
{
    private ItemGroupAccess _itemGroupAccess;

    public ItemGroupServices(ItemGroupAccess itemGroupAccess)
    {
        _itemGroupAccess = itemGroupAccess;
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

    public async Task<bool> RemoveItemGroup(int itemGroupId) => await _itemGroupAccess.Remove(itemGroupId);
}