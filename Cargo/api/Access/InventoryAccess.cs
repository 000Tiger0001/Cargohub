using Microsoft.EntityFrameworkCore;
public class InventoryAccess : BaseAccess<Inventory>
{
    public InventoryAccess(ApplicationDbContext context) : base(context) { }

    public async Task<Inventory?> GetInventoryByItemId(int itemId) => await DB.AsNoTracking().FirstOrDefaultAsync(entity => entity.ItemId == itemId);

}