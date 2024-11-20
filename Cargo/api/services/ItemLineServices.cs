public class ItemLineServices
{
    public async Task<List<ItemLine>> GetItemLines() => await AccessJson.ReadJson<ItemLine>();

    public async Task<ItemLine> GetItemLine(Guid itemLineId)
    {
        List<ItemLine> itemLines = await GetItemLines();
        return itemLines.FirstOrDefault(i => i.Id == itemLineId)!;
    }

    public async Task<bool> AddItemLine(ItemLine itemLine)
    {
        if (itemLine is null || itemLine.Name == "") return false;
        List<ItemLine> itemLines = await GetItemLines();
        ItemLine doubleItemLine = itemLines.Find(i => i.Name == itemLine.Name)!;
        if (doubleItemLine is null) return false;
        await AccessJson.WriteJson(itemLine);
        return true;
    }

    public async Task<bool> UpdateItemLine(ItemLine itemLine)
    {
        if (itemLine is null) return false;
        List<ItemLine> itemLines = await GetItemLines();
        int itemLineIndex = itemLines.FindIndex(i => i.Id == itemLine.Id);
        if (itemLineIndex == -1) return false;
        itemLine.UpdatedAt = DateTime.Now;
        itemLines[itemLineIndex] = itemLine;
        AccessJson.WriteJsonList(itemLines);
        return true;
    }

    public async Task<bool> RemoveItemLine(Guid itemLineId)
    {
        if (itemLineId == Guid.Empty) return false;
        List<ItemLine> itemLines = await GetItemLines();
        ItemLine foundItemLine = itemLines.FirstOrDefault(i => i.Id == itemLineId)!;
        if (foundItemLine is null) return false;
        itemLines.Remove(foundItemLine);
        AccessJson.WriteJsonList(itemLines);
        return true;
    }
}