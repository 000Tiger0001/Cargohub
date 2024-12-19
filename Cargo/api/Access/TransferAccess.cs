using Microsoft.EntityFrameworkCore;

public class TransferAccess : BaseAccess<Transfer>
{
    public TransferAccess(ApplicationDbContext context) : base(context) { }

    public override async Task<List<Transfer>> GetAll()
    {
        List<Transfer> transfers = await _context.Set<Transfer>().AsNoTracking()
            .Include(transfer => transfer.Items)
            .ToListAsync();
        return transfers;
    }

    public override async Task<Transfer?> GetById(int transferId)
    {
        Transfer? transfer = await _context.Set<Transfer>().AsNoTracking()
            .Include(transfer => transfer.Items)
            .FirstOrDefaultAsync(transfer => transfer.Id == transferId);
        return transfer;
    }

    public override async Task<bool> Update(Transfer transfer)
    {
        if (transfer == null) return false;

        DetachEntity(transfer);

        var existingTransfer = await GetById(transfer.Id!);
        if (existingTransfer == null) return false;

        if (existingTransfer.Items != null)
        {
            foreach (TransferItemMovement item in existingTransfer.Items)
            {
                var existingItem = await _context.Set<TransferItemMovement>().FirstOrDefaultAsync(i => i.Id == item.Id);
                if (existingItem != null)
                {
                    existingItem.Amount = item.Amount;
                    existingItem.ItemId = item.ItemId;
                }
                else _context.Set<TransferItemMovement>().Add(item);
            }
        }
        _context.Update(transfer);
        var changes = await _context.SaveChangesAsync();
        ClearChangeTracker();
        return changes > 0;
    }
}