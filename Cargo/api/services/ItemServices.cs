using SQLitePCL;

public class ItemServices
{
    private readonly ItemAccess _itemAccess;
    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly TransferItemMovementAccess _transferItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;

    public ItemServices(ItemAccess itemAccess, OrderItemMovementAccess orderItemMovementAccess, TransferItemMovementAccess transferItemMovementAccess, ShipmentItemMovementAccess shipmentItemMovementAccess)
    {
        _itemAccess = itemAccess;
        _orderItemMovementAccess = orderItemMovementAccess;
        _transferItemMovementAccess = transferItemMovementAccess;
        _shipmentItemMovementAccess = shipmentItemMovementAccess;
    }

    public async Task<List<Item>> GetItems() => await _itemAccess.GetAll();

    public async Task<Item?> GetItem(int itemId) => await _itemAccess.GetById(itemId);

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
        return await _itemAccess.Add(item);
    }

    public async Task<bool> UpdateItem(Item item)
    {
        if (item is null || item.Id == 0) return false;
        item.UpdatedAt = DateTime.Now;
        return await _itemAccess.Update(item);
    }

    public async Task<bool> RemoveItem(int itemId)
    {
        bool IsRemoved1 = await _shipmentItemMovementAccess.Remove(itemId);
        bool IsRemoved2 = await _orderItemMovementAccess.Remove(itemId);
        bool IsRemoved3 = await _transferItemMovementAccess.Remove(itemId);
        return await _itemAccess.Remove(itemId);
    }
}