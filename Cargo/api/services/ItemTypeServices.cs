public class ItemTypeServices
{
    private ItemTypeAccess _itemTypeAccess;
    private bool _debug;
    private List<ItemType> _testItemTypes;

    public ItemTypeServices(ItemTypeAccess itemTypeAccess, bool debug)
    {
        _itemTypeAccess = itemTypeAccess;
        _debug = debug;
        _testItemTypes = [];
    }

    public async Task<List<ItemType>> GetItemTypes()
    {
        if (!_debug) return await _itemTypeAccess.GetAll();
        return _testItemTypes;
    }

    public async Task<ItemType?> GetItemType(int itemTypeId)
    {
        if (!_debug) return await _itemTypeAccess.GetById(itemTypeId);
        return _testItemTypes.FirstOrDefault(i => i.Id == itemTypeId);
    }

    public async Task<bool> AddItemType(ItemType itemType)
    {
        if (itemType is null || itemType.Name == "") return false;
        List<ItemType> itemTypes = await GetItemTypes();
        ItemType doubleItemType = itemTypes.FirstOrDefault(i => i.Name == itemType.Name)!;
        if (doubleItemType is null) return false;
        if (!_debug) return await _itemTypeAccess.Add(itemType);
        _testItemTypes.Add(itemType);
        return true;
    }

    public async Task<bool> UpdateItemType(ItemType itemType)
    {
        if (itemType is null || itemType.Id == 0) return false;
        itemType.UpdatedAt = DateTime.Now;
        if (!_debug) return await _itemTypeAccess.Update(itemType);
        int foundItemTypeIndex = _testItemTypes.FindIndex(i => i.Id == itemType.Id);
        if (foundItemTypeIndex == -1) return false;
        _testItemTypes[foundItemTypeIndex] = itemType;
        return true;
    }

    public async Task<bool> RemoveItemType(int itemTypeId)
    {
        if (!_debug) return await _itemTypeAccess.Remove(itemTypeId);
        return _testItemTypes.Remove(_testItemTypes.FirstOrDefault(i => i.Id == itemTypeId)!);
    }
}