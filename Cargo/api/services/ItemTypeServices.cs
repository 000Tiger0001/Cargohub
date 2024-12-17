public class ItemTypeServices
{
    private readonly ItemTypeAccess _itemTypeAccess;
    private readonly ItemAccess _itemAccess;

    public ItemTypeServices(ItemTypeAccess itemTypeAccess, ItemAccess itemAccess)
    {
        _itemTypeAccess = itemTypeAccess;
        _itemAccess = itemAccess;
    }

    public async Task<List<ItemType>> GetItemTypes() => await _itemTypeAccess.GetAll();

    public async Task<ItemType?> GetItemType(int itemTypeId) => await _itemTypeAccess.GetById(itemTypeId);

    public async Task<bool> AddItemType(ItemType itemType)
    {
        if (itemType is null || itemType.Name == "") return false;
        List<ItemType> itemTypes = await GetItemTypes();
        if (itemTypes.FirstOrDefault(i => i.Name == itemType.Name) is not null) return false;
        return await _itemTypeAccess.Add(itemType);
    }

    public async Task<bool> UpdateItemType(ItemType itemType)
    {
        if (itemType is null || itemType.Id <= 0) return false;
        itemType.UpdatedAt = DateTime.Now;
        return await _itemTypeAccess.Update(itemType);
    }

    public async Task<bool> RemoveItemType(int itemTypeId)
    {
        List<Item> items = await _itemAccess.GetAll();
        items.ForEach(i => { if (itemTypeId == i.ItemTypeId) i.ItemTypeId = 0; });
        await _itemAccess.UpdateMany(items);
        return await _itemTypeAccess.Remove(itemTypeId);
    }
}