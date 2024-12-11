public class TransferItemMovementServices
{
    private readonly TransferItemMovementAccess _access;

    public TransferItemMovementServices(TransferItemMovementAccess transferItemMovementAccess)
    {
        _access = transferItemMovementAccess;
    }

    public async Task<List<TransferItemMovement>> GetTransferItemMovements() => await _access.GetAll();

    public async Task<TransferItemMovement?> GetTransferItemMovement(int transferItemMovementId) => await _access.GetById(transferItemMovementId);

    public async Task<bool> AddTransferItemMovement(TransferItemMovement transferItemMovement)
    {
        List<TransferItemMovement> transferItemMovements = await GetTransferItemMovements();
        TransferItemMovement doubleTransferItemMovement = transferItemMovements.FirstOrDefault(t => t.Id == transferItemMovement.Id || (t.ItemId == transferItemMovement.ItemId && t.Amount == transferItemMovement.Amount))!;
        if (doubleTransferItemMovement is not null) return false;
        return await _access.Add(transferItemMovement);
    }

    public async Task<bool> UpdateTransferItemMovement(TransferItemMovement transferItemMovement)
    {
        if (transferItemMovement is null || transferItemMovement.Id == 0 || transferItemMovement.ItemId == 0 || transferItemMovement.Amount == 0) return false;
        return await _access.Update(transferItemMovement);
    }

    public async Task<bool> RemoveTransferItemMovement(int transferItemMovementId) => await _access.Remove(transferItemMovementId);
}