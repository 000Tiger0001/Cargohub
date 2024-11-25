public class ItemLineServices
{
    private ItemLineAccess _itemLineAccess;
    private bool _debug;
    private List<ItemLine> _testItemLines;

    public ItemLineServices(ItemLineAccess itemLineAccess, bool debug)
    {
        _itemLineAccess = itemLineAccess;
        _debug = debug;
        _testItemLines = [];
    }
    public async Task<List<ItemLine>> GetItemLines() => _debug ? _testItemLines : await _itemLineAccess.GetAll();

    public async Task<ItemLine?> GetItemLine(int itemLineId) => _debug ? _testItemLines.FirstOrDefault(i => i.Id == itemLineId) : await _itemLineAccess.GetById(itemLineId);

    public async Task<bool> AddItemLine(ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Name == "") return false;
        List<ItemLine> itemLines = await GetItemLines();
        ItemLine doubleItemLine = itemLines.Find(i => i.Name == itemLine.Name)!;
        if (doubleItemLine is not null) return false;
        if (!_debug) return await _itemLineAccess.Add(itemLine);
        _testItemLines.Add(itemLine);
        return true;
    }

    public async Task<bool> UpdateItemLine(ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Id == 0) return false;
        itemLine.UpdatedAt = DateTime.Now;
        if (!_debug) return await _itemLineAccess.Update(itemLine);
        int foundItemLineIndex = _testItemLines.FindIndex(i => i.Id == itemLine.Id);
        if (foundItemLineIndex == -1) return false;
        _testItemLines[foundItemLineIndex] = itemLine;
        return true;
    }

    public async Task<bool> RemoveItemLine(int itemLineId) => _debug ? _testItemLines.Remove(_testItemLines.FirstOrDefault(i => i.Id == itemLineId)!) : await _itemLineAccess.Remove(itemLineId);
}