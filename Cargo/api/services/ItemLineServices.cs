public class ItemLineServices
{
    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemAccess _itemAccess;

    public ItemLineServices(ItemLineAccess itemLineAccess, ItemAccess itemAccess)
    {
        _itemLineAccess = itemLineAccess;
        _itemAccess = itemAccess;
    }
    public async Task<List<ItemLine>> GetItemLines() => await _itemLineAccess.GetAll();

    public async Task<ItemLine?> GetItemLine(int itemLineId) => await _itemLineAccess.GetById(itemLineId);

    public async Task<bool> AddItemLine(ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Name == "") return false;
        List<ItemLine> itemLines = await GetItemLines();
        if (itemLines.FirstOrDefault(i => i.Name == itemLine.Name) is not null) return false;
        return await _itemLineAccess.Add(itemLine);
    }

    public async Task<bool> UpdateItemLine(ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Id <= 0) return false;
        itemLine.UpdatedAt = DateTime.Now;
        return await _itemLineAccess.Update(itemLine);
    }

    public async Task<bool> RemoveItemLine(int itemLineId)
    {
        List<Item> items = await _itemAccess.GetAll();
        items.ForEach(i => { if (itemLineId == i.ItemLineId) i.ItemLineId = 0; });
        await _itemAccess.UpdateMany(items);
        return await _itemLineAccess.Remove(itemLineId);
    }
}