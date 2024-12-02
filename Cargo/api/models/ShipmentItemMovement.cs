public class ShipmentItemMovement : ItemMovement
{
    public ShipmentItemMovement(int itemId, int amount) : base(itemId, amount) {}

    public override bool Equals(object? obj)
    {
        if (obj is ShipmentItemMovement shipmentItemMovement) return shipmentItemMovement.ItemId == ItemId && shipmentItemMovement.Amount == Amount;
        return false;
    }

    public override int GetHashCode() => ItemId.GetHashCode();
}
