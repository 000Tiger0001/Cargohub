public class ItemServices
{
    public async Task<List<Item>> GetItems()
    {
        List<Item> items = await AccessJson.ReadJson<Item>();
        return items;
    }

    public async Task<Item> GetItem(Guid itemId)
    {
        List<Item> items = await GetItems();
        return items.FirstOrDefault(i => i.Id == itemId)!;
    }

    public async Task<List<Item>> GetItemsForItemLine(Guid itemLineId)
    {
        List<Item> items = await GetItems();
        List<Item> itemsWithItemLineId = items.FindAll(i => i.ItemLineId == itemLineId);
        return itemsWithItemLineId;
    }

    public async Task<List<Item>> GetItemsForItemGroup(Guid itemGroupId)
    {
        List<Item> items = await GetItems();
        List<Item> itemsWithItemGroupId = items.FindAll(i => i.ItemGroupId == itemGroupId);
        return itemsWithItemGroupId;
    }

    public async Task<List<Item>> GetItemsForItemType(Guid itemTypeId)
    {
        List<Item> items = await GetItems();
        List<Item> itemsWithItemTypeId = items.FindAll(i => i.ItemTypeId == itemTypeId);
        return itemsWithItemTypeId;
    }

    public async Task<List<Item>> GetItemsForSupplier(Guid supplierId)
    {
        List<Item> items = await GetItems();
        List<Item> itemsWithSupplierId = items.FindAll(i => i.SupplierId == supplierId);
        return itemsWithSupplierId;
    }

    public async Task<bool> AddItem(Item item)
    {
        if (item is null || item.Code == "" || item.CommodityCode == "" || item.Description == "" || item.ItemGroupId == default || item.ItemLineId == default || item.ItemTypeId == default || item.ModelNumber == "" || item.UpcCode == "" || item.ShortDescription == "") return false;
        item.Id = Guid.NewGuid();
        List<Item> items = await GetItems();
        Item doubleItem = items.FirstOrDefault(i => i.Code == item.Code && i.CommodityCode == i.CommodityCode && i.Description == item.Description && i.ShortDescription == item.ShortDescription && i.ItemGroupId == item.ItemGroupId && i.ItemLineId == item.ItemLineId && i.ItemTypeId == item.ItemTypeId && i.ModelNumber == item.ModelNumber && i.UpcCode == item.UpcCode)!;
        if (doubleItem is not null) return false;
        await AccessJson.WriteJson(item);
        return true;
    }

    public async Task<bool> UpdateItem(Item item)
    {
        if (item is null) return false;
        List<Item> items = await GetItems();
        int foundItemIndex = items.FindIndex(i => i.Id == item.Id);
        if (foundItemIndex == -1) return false;
        item.UpdatedAt = DateTime.Now;
        items[foundItemIndex] = item;
        AccessJson.WriteJsonList(items);
        return true;
    }

    public async Task<bool> RemoveItem(Guid itemId)
    {
        if (itemId == Guid.Empty) return false;
        List<Item> items = await GetItems();
        Item foundItem = items.FirstOrDefault(i => i.Id == itemId)!;
        if (foundItem is null) return false;
        items.Remove(foundItem);
        AccessJson.WriteJsonList(items);
        return true;
    }
}