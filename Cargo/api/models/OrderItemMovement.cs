using System.ComponentModel.DataAnnotations.Schema;

public class OrderItemMovement : ItemMovement
{
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }

    public OrderItemMovement(int itemId, int amount) : base(itemId, amount) { }
    public OrderItemMovement(int id, int itemId, int amount) : base(id, itemId, amount) { }

    public override bool Equals(object? obj)
    {
        if (obj is OrderItemMovement orderItemMovement) return orderItemMovement.ItemId == ItemId && orderItemMovement.Amount == Amount;
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(ItemId, Amount);
}
