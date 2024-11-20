public class TransferServices
{
    public async Task<List<Transfer>> GetTransfers() => await AccessJson.ReadJson<Transfer>();

    public async Task<Transfer> GetTransfer(Guid transferId)
    {
        List<Transfer> transfers = await GetTransfers();
        return transfers.FirstOrDefault(t => t.Id == transferId)!;
    }

    public async Task<Dictionary<Guid, int>> GetItemsInTransfer(Guid transferId)
    {
        Transfer transfer = await GetTransfer(transferId);
        if (transfer is null) return default!;

        return transfer.Items;
    }

    public async Task<bool> AddTransfer(Transfer transfer)
    {
        if (transfer is null || transfer.Reference == "" || (transfer.TransferFrom == Guid.Empty && transfer.TransferTo == Guid.Empty) || transfer.TransferStatus == "") return false;

        List<Transfer> transfers = await GetTransfers();
        Transfer doubleTransfer = transfers.FirstOrDefault(t => t.Reference == transfer.Reference && t.TransferFrom == transfer.TransferFrom && t.TransferTo == transfer.TransferTo && t.TransferStatus == transfer.TransferStatus)!;
        if (doubleTransfer is null) return false;

        transfer.Id = Guid.NewGuid();
        await AccessJson.WriteJson(transfer);
        return true;
    }

    public async Task<bool> UpdateTransfer(Transfer transfer)
    {
        if (transfer is null || transfer.Id == Guid.Empty) return false;

        List<Transfer> transfers = await GetTransfers();
        int foundTransferIndex = transfers.FindIndex(t => t.Id == transfer.Id);
        if (foundTransferIndex == -1) return false;

        transfer.UpdatedAt = DateTime.Now;
        transfers[foundTransferIndex] = transfer;
        AccessJson.WriteJsonList(transfers);
        return true;
    }

    public async Task<bool> RemoveTransfer(Guid transferId)
    {
        if (transferId == Guid.Empty) return false;

        List<Transfer> transfers = await GetTransfers();
        Transfer foundTransfer = transfers.FirstOrDefault(t => t.Id == transferId)!;
        if (foundTransfer is null) return false;

        transfers.Remove(foundTransfer);
        AccessJson.WriteJsonList(transfers);
        return true;
    }
}