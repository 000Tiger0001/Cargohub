using System.ComponentModel.DataAnnotations.Schema;


public class TransferItemMovement : ItemMovement
{
    public int TransferId { get; set; }

    [ForeignKey("TransferId")]
    public virtual Transfer? Transfer { get; set; }

    public TransferItemMovement(int itemId, int amount) : base(itemId, amount) {}

    public override bool Equals(object? obj)
    {
        if (obj is TransferItemMovement transferItemMovement) return transferItemMovement.ItemId == ItemId && transferItemMovement.Amount == Amount;
        return false;
    }

    public override int GetHashCode() => ItemId.GetHashCode();
}
