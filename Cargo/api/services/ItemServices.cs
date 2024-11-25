public class ItemServices
{
    private ItemAccess _itemAccess;
    private bool _debug;
    private List<Item> _testItems;
    public ItemServices(ItemAccess itemAccess, bool debug)
    {
        _itemAccess = itemAccess;
        _debug = debug;
        _testItems = [];
    }

    public async Task<List<Item>> GetItems()
    {
        if (!_debug) return await _itemAccess.GetAll();
        return _testItems;
    }

    public async Task<Item?> GetItem(int itemId)
    {
        if (!_debug) return await _itemAccess.GetById(itemId);
        return _testItems.FirstOrDefault(i => i.Id == itemId);
    }

    public async Task<List<Item>> GetItemsForItemLine(int itemLineId)
    {
        List<Item> items = await GetItems();
        return items.FindAll(i => i.ItemLineId == itemLineId);
    }

    public async Task<List<Item>> GetItemsForItemGroup(int itemGroupId)
    {
        List<Item> items = await GetItems();
        return items.FindAll(i => i.ItemGroupId == itemGroupId);
    }

    public async Task<List<Item>> GetItemsForItemType(int itemTypeId)
    {
        List<Item> items = await GetItems();
        return items.FindAll(i => i.ItemTypeId == itemTypeId);
    }

    public async Task<List<Item>> GetItemsForSupplier(int supplierId)
    {
        List<Item> items = await GetItems();
        return items.FindAll(i => i.SupplierId == supplierId);
    }

    public async Task<bool> AddItem(Item item)
    {
        if (item is null || item.Code == "" || item.CommodityCode == "" || item.Description == "" || item.ItemGroupId == default || item.ItemLineId == default || item.ItemTypeId == default || item.ModelNumber == "" || item.UpcCode == "" || item.ShortDescription == "") return false;
        List<Item> items = await GetItems();
        Item doubleItem = items.FirstOrDefault(i => i.Code == item.Code && i.CommodityCode == i.CommodityCode && i.Description == item.Description && i.ShortDescription == item.ShortDescription && i.ItemGroupId == item.ItemGroupId && i.ItemLineId == item.ItemLineId && i.ItemTypeId == item.ItemTypeId && i.ModelNumber == item.ModelNumber && i.UpcCode == item.UpcCode)!;
        if (doubleItem is not null) return false;
        if (!_debug) return await _itemAccess.Add(item);
        _testItems.Add(item);
        return true;
    }

    public async Task<bool> UpdateItem(Item item)
    {
        if (item is null || item.Id == 0) return false;
        item.UpdatedAt = DateTime.Now;
        if (!_debug) return await _itemAccess.Update(item);
        int foundItemIndex = _testItems.FindIndex(i => i.Id == item.Id);
        if (foundItemIndex == -1) return false;
        _testItems[foundItemIndex] = item;
        return true;
    }

    public async Task<bool> RemoveItem(int itemId)
    {
        if (!_debug) return await _itemAccess.Remove(itemId);
        return _testItems.Remove(_testItems.FirstOrDefault(i => i.Id == itemId)!);
    }
}