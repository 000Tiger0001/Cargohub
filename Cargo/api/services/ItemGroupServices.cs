public class ItemGroupServices
{
    private ItemGroupAccess _itemGroupAccess;
    private bool _debug;
    private List<ItemGroup> _testItemGroups;

    public ItemGroupServices(ItemGroupAccess itemGroupAccess, bool debug)
    {
        _itemGroupAccess = itemGroupAccess;
        _debug = debug;
        if (_debug) _testItemGroups = [];
        else _testItemGroups = null!;
    }

    public async Task<List<ItemGroup>> GetItemGroups()
    {
        if (!_debug) return await _itemGroupAccess.GetAll();
        return _testItemGroups;
    }

    public async Task<ItemGroup?> GetItemGroup(int itemGroupId) 
    {
        if (!_debug) return await _itemGroupAccess.GetById(itemGroupId);
        return _testItemGroups.FirstOrDefault(i => i.Id == itemGroupId);
    }

    public async Task<bool> AddItemGroup(ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Name == "") return false;
        List<ItemGroup> itemGroups = await GetItemGroups();
        ItemGroup doubleItemGroup = itemGroups.Find(i => i.Name == itemGroup.Name)!;
        if (doubleItemGroup is not null) return false;
        if (!_debug) return await _itemGroupAccess.Add(itemGroup);
        _testItemGroups.Add(itemGroup);
        return true;
    }

    public async Task<bool> UpdateItemGroup(ItemGroup itemGroup)
    {
        if (itemGroup is null || itemGroup.Id == 0) return false;
        itemGroup.UpdatedAt = DateTime.Now;
        if (!_debug) return await _itemGroupAccess.Update(itemGroup);
        int foundItemGroupIndex = _testItemGroups.FindIndex(i => i.Id == itemGroup.Id);
        if (foundItemGroupIndex == -1) return false;
        _testItemGroups[foundItemGroupIndex] = itemGroup;
        return true;
    }

    public async Task<bool> RemoveItemGroup(int itemGroupId)
    {
        if (!_debug) return await _itemGroupAccess.Remove(itemGroupId);
        return _testItemGroups.Remove(_testItemGroups.FirstOrDefault(i => i.Id == itemGroupId)!);
    }
}