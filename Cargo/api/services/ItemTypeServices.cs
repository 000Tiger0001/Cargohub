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
        if (doubleItemType is not null) return false;
        return await _itemTypeAccess.Add(itemType);
    }

    public async Task<bool> UpdateItemType(ItemType itemType)
    {
        if (itemType is null || itemType.Id == 0) return false;
        itemType.UpdatedAt = DateTime.Now;
        return await _itemTypeAccess.Update(itemType);
    }

    public async Task<bool> RemoveItemType(int itemTypeId) => await _itemTypeAccess.Remove(itemTypeId);
}