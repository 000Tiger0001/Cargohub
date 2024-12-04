public class OrderItemMovement : ItemMovement
{
    public OrderItemMovement(int itemId, int amount) : base(itemId, amount) { }

    public override bool Equals(object? obj)
    {
        if (obj is OrderItemMovement orderItemMovement) return orderItemMovement.ItemId == ItemId && orderItemMovement.Amount == Amount;
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(ItemId, Amount);
}
