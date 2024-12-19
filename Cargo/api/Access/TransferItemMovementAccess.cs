using Microsoft.EntityFrameworkCore;

public class TransferItemMovementAccess : BaseAccess<TransferItemMovement>
{
    public TransferItemMovementAccess(ApplicationDbContext context) : base(context) { }

    public async Task<List<TransferItemMovement>> GetAllByOrderId(int transferId)
    {
        return await DB.AsNoTracking().Where(entity => entity.TransferId == transferId).ToListAsync();
    }
}