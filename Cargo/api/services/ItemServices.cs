using SQLitePCL;

public class ItemServices
{
    private readonly ItemAccess _itemAccess;
    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly TransferItemMovementAccess _transferItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private readonly ItemGroupAccess _itemGroupAccess;
    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemTypeAccess _itemTypeAccess;
    private readonly SupplierAccess _supplierAccess;

    public ItemServices(ItemAccess itemAccess, OrderItemMovementAccess orderItemMovementAccess, TransferItemMovementAccess transferItemMovementAccess, ShipmentItemMovementAccess shipmentItemMovementAccess, ItemGroupAccess itemGroupAccess, ItemLineAccess itemLineAccess, ItemTypeAccess itemTypeAccess, SupplierAccess supplierAccess)
    {
        _itemAccess = itemAccess;
        _orderItemMovementAccess = orderItemMovementAccess;
        _transferItemMovementAccess = transferItemMovementAccess;
        _shipmentItemMovementAccess = shipmentItemMovementAccess;
        _itemGroupAccess = itemGroupAccess;
        _itemLineAccess = itemLineAccess;
        _itemTypeAccess = itemTypeAccess;
        _supplierAccess = supplierAccess;
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
        ItemGroup? foundItemGroup = await _itemGroupAccess.GetById(int.Parse(item.ItemGroupId.ToString()!));
        ItemLine? foundItemLine = await _itemLineAccess.GetById(int.Parse(item.ItemLineId.ToString()!));
        ItemType? foundItemType = await _itemTypeAccess.GetById(int.Parse(item.ItemTypeId.ToString()!));
        Supplier? foundSupplier = await _supplierAccess.GetById(item.SupplierId);
        if (doubleItem is not null || foundItemGroup is null || foundItemLine is null || foundItemType is null || foundSupplier is null) return false;
        return await _itemAccess.Add(item);
    }

    public async Task<bool> UpdateItem(Item item)
    {
        if (item is null || item.Id <= 0) return false;
        item.UpdatedAt = DateTime.Now;
        return await _itemAccess.Update(item);
    }

    public async Task<bool> RemoveItem(int itemId)
    {
        List<ShipmentItemMovement> shipmentItemMovement = await _shipmentItemMovementAccess.GetAll();
        List<OrderItemMovement> orderItemMovements = await _orderItemMovementAccess.GetAll();
        List<TransferItemMovement> transferItemMovements = await _transferItemMovementAccess.GetAll();
        List<ShipmentItemMovement> shipmentItemMovementsToDelete = shipmentItemMovement.Where(s => s.ItemId == itemId).ToList();
        List<OrderItemMovement> orderItemMovementsToDelete = orderItemMovements.Where(o => o.ItemId == itemId).ToList();
        List<TransferItemMovement> transferItemMovementsToDelete = transferItemMovements.Where(t => t.ItemId == itemId).ToList();
        foreach (ShipmentItemMovement shipmentItem in shipmentItemMovementsToDelete) await _shipmentItemMovementAccess.Remove(itemId);
        foreach (OrderItemMovement orderItem in orderItemMovementsToDelete) await _orderItemMovementAccess.Remove(itemId);
        foreach (TransferItemMovement transferItem in transferItemMovementsToDelete) await _transferItemMovementAccess.Remove(itemId);
        return await _itemAccess.Remove(itemId);
    }
}