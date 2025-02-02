using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

public class ShipmentItemMovement : ItemMovement
{
    public int Shipment_Id { get; set; }

    [ForeignKey("Shipment_Id")]
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual Shipment? Shipment { get; set; }

    public ShipmentItemMovement() { }

    public ShipmentItemMovement(int itemId, int amount) : base(itemId, amount) { }
    public ShipmentItemMovement(int id, int itemId, int amount) : base(id, itemId, amount) { }

    public override bool Equals(object? obj)
    {
        if (obj is ShipmentItemMovement shipmentItemMovement) return shipmentItemMovement.ItemId == ItemId && shipmentItemMovement.Amount == Amount;
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(ItemId, Amount);
}
