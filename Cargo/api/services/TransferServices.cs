public class TransferServices
{
    private TransferAccess _transferAccess;

    public TransferServices(TransferAccess transferAccess)
    {
        _transferAccess = transferAccess;
    }
    public async Task<List<Transfer>> GetTransfers() => await _transferAccess.GetAll();

    public async Task<Transfer?> GetTransfer(int transferId) => await _transferAccess.GetById(transferId)!;

    public async Task<List<TransferItemMovement>?> GetItemsInTransfer(int transferId)
    {
        Transfer? transfer = await GetTransfer(transferId)!;
        if (transfer is null) return default!;

        return transfer.Items;
    }

    public async Task<bool> AddTransfer(Transfer transfer)
    {
        if (transfer is null || transfer.Reference == "" || (transfer.TransferFrom == 0 && transfer.TransferTo == 0) || transfer.TransferStatus == "") return false;

        List<Transfer> transfers = await GetTransfers();
        Transfer doubleTransfer = transfers.FirstOrDefault(t => t.Reference == transfer.Reference && t.TransferFrom == transfer.TransferFrom && t.TransferTo == transfer.TransferTo && t.TransferStatus == transfer.TransferStatus)!;
        if (doubleTransfer is null) return false;

        bool IsAdded = await _transferAccess.Add(transfer);
        return IsAdded;
    }

    public async Task<bool> UpdateTransfer(Transfer transfer)
    {
        if (transfer is null || transfer.Id == 0) return false;

        transfer.UpdatedAt = DateTime.Now;
        bool IsUpdated = await _transferAccess.Update(transfer);
        return IsUpdated;
    }

    public async Task<bool> RemoveTransfer(int transferId) => await _transferAccess.Delete(transferId);
}