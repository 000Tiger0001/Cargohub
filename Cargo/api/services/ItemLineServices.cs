public class ItemLineServices
{
    private ItemLineAccess _itemLineAccess;

    public ItemLineServices(ItemLineAccess itemLineAccess)
    {
        _itemLineAccess = itemLineAccess;
    }
    public async Task<List<ItemLine>> GetItemLines() => await _itemLineAccess.GetAll();

    public async Task<ItemLine?> GetItemLine(int itemLineId) => await _itemLineAccess.GetById(itemLineId);

    public async Task<bool> AddItemLine(ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Name == "") return false;
        List<ItemLine> itemLines = await GetItemLines();
        ItemLine doubleItemLine = itemLines.Find(i => i.Name == itemLine.Name)!;
        if (doubleItemLine is not null) return false;
        return await _itemLineAccess.Add(itemLine);
    }

    public async Task<bool> UpdateItemLine(ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Id == 0) return false;
        itemLine.UpdatedAt = DateTime.Now;
        return await _itemLineAccess.Update(itemLine);
    }

    public async Task<bool> RemoveItemLine(int itemLineId) => await _itemLineAccess.Remove(itemLineId);
}