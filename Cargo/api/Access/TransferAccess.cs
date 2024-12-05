public class TransferAccess : BaseAccess<Transfer>
{
    private readonly ItemMovementAccess<Transfer, TransferItemMovement> _itemMovementAccess;

    public TransferAccess(ApplicationDbContext context) : base(context)
    {
        _itemMovementAccess = new ItemMovementAccess<Transfer, TransferItemMovement>(context);
    }

    public override async Task<List<Transfer>> GetAll()
    {
        return await _itemMovementAccess.GetAll();
    }

    public override async Task<Transfer?> GetById(int id)
    {
        return await _itemMovementAccess.GetById(id);
    }

    public override async Task<bool> Update(Transfer transfer)
    {
        return await _itemMovementAccess.Update(transfer);
    }
}