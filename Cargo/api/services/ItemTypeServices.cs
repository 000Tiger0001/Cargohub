public class ItemTypeServices
{
    private ItemTypeAccess _itemTypeAccess;

    public ItemTypeServices(ItemTypeAccess itemTypeAccess)
    {
        _itemTypeAccess = itemTypeAccess;
    }

    public async Task<List<ItemType>> GetItemTypes() => await _itemTypeAccess.GetAll();

    public async Task<ItemType?> GetItemType(int itemTypeId) => await _itemTypeAccess.GetById(itemTypeId);

    public async Task<bool> AddItemType(ItemType itemType)
    {
        if (itemType is null || itemType.Name == "") return false;
        List<ItemType> itemTypes = await GetItemTypes();
        ItemType doubleItemType = itemTypes.FirstOrDefault(i => i.Name == itemType.Name)!;
        if (doubleItemType is null) return false;
        bool IsAdded = await _itemTypeAccess.Add(itemType);
        return IsAdded;
    }

    public async Task<bool> UpdateItemType(ItemType itemType)
    {
        if (itemType is null || itemType.Id == 0) return false;

        itemType.UpdatedAt = DateTime.Now;
        bool IsUpdated = await _itemTypeAccess.Update(itemType);
        return IsUpdated;
    }

    public async Task<bool> RemoveItemType(int itemTypeId) => await _itemTypeAccess.Delete(itemTypeId);
}