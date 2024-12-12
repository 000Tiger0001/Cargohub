public class TransferServices
{
    private readonly TransferAccess _transferAccess;
    private readonly ItemAccess _itemAccess;

    public TransferServices(TransferAccess transferAccess, ItemAccess itemAccess)
    {
        _transferAccess = transferAccess;
        _itemAccess = itemAccess;
    }
    public async Task<List<Transfer>> GetTransfers() => await _transferAccess.GetAll();

    public async Task<Transfer?> GetTransfer(int transferId) => await _transferAccess.GetById(transferId)!;

    public async Task<List<TransferItemMovement>?> GetItemsInTransfer(int transferId)
    {
        Transfer? transfer = await GetTransfer(transferId)!;
        if (transfer is null) return [];
        return transfer.Items;
    }

    public async Task<bool> AddTransfer(Transfer transfer)
    {
        if (transfer is null || transfer.Reference == "" || transfer.TransferStatus == "") return false;
        List<Transfer> transfers = await GetTransfers();
        Transfer doubleTransfer = transfers.FirstOrDefault(t => t.Reference == transfer.Reference && t.TransferFrom == transfer.TransferFrom && t.TransferTo == transfer.TransferTo && t.TransferStatus == transfer.TransferStatus)!;
        foreach (TransferItemMovement transferItemMovement in transfer.Items!) if (await _itemAccess.GetById(transferItemMovement.ItemId) is null) return false;
        if (doubleTransfer is not null) return false;
        return await _transferAccess.Add(transfer);
    }

    public async Task<bool> UpdateTransfer(Transfer transfer)
    {
        if (transfer is null || transfer.Id == 0) return false;
        transfer.UpdatedAt = DateTime.Now;
        return await _transferAccess.Update(transfer);
    }

    public async Task<bool> RemoveTransfer(int transferId) => await _transferAccess.Remove(transferId);
}