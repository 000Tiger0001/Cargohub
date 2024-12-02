public class TransferItemMovement : ItemMovement
{
    public TransferItemMovement(int itemId, int amount) : base(itemId, amount) {}

    public override bool Equals(object? obj)
    {
        if (obj is TransferItemMovement transferItemMovement) return transferItemMovement.ItemId == ItemId && transferItemMovement.Amount == Amount;
        return false;
    }

    public override int GetHashCode() => ItemId.GetHashCode();
}
