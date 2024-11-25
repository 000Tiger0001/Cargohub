public class TransferServices
{
    private TransferAccess _transferAccess;
    private bool _debug;
    private List<Transfer> _testTransfers;

    public TransferServices(TransferAccess transferAccess, bool debug)
    {
        _transferAccess = transferAccess;
        _debug = debug;
        _testTransfers = [];
    }
    public async Task<List<Transfer>> GetTransfers() => _debug ? _testTransfers : await _transferAccess.GetAll();

    public async Task<Transfer?> GetTransfer(int transferId) => _debug ? _testTransfers.FirstOrDefault(t => t.Id == transferId) : await _transferAccess.GetById(transferId)!;

    public async Task<List<TransferItemMovement>?> GetItemsInTransfer(int transferId)
    {
        Transfer? transfer = await GetTransfer(transferId)!;
        if (transfer is null) return [];
        return transfer.Items;
    }

    public async Task<bool> AddTransfer(Transfer transfer)
    {
        if (transfer is null || transfer.Reference == "" || (transfer.TransferFrom == 0 && transfer.TransferTo == 0) || transfer.TransferStatus == "") return false;
        List<Transfer> transfers = await GetTransfers();
        Transfer doubleTransfer = transfers.FirstOrDefault(t => t.Reference == transfer.Reference && t.TransferFrom == transfer.TransferFrom && t.TransferTo == transfer.TransferTo && t.TransferStatus == transfer.TransferStatus)!;
        if (doubleTransfer is null) return false;
        if (!_debug) return await _transferAccess.Add(transfer);
        _testTransfers.Add(transfer);
        return true;
    }

    public async Task<bool> UpdateTransfer(Transfer transfer)
    {
        if (transfer is null || transfer.Id == 0) return false;
        transfer.UpdatedAt = DateTime.Now;
        if (!_debug) return await _transferAccess.Update(transfer);
        int foundTransferIndex = _testTransfers.FindIndex(t => t.Id == transfer.Id);
        if (foundTransferIndex == -1) return false;
        _testTransfers[foundTransferIndex] = transfer;
        return true;
    }

    public async Task<bool> RemoveTransfer(int transferId) => _debug ? _testTransfers.Remove(_testTransfers.FirstOrDefault(t => t.Id == transferId)!): await _transferAccess.Remove(transferId);
}