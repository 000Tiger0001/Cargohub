using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

public class OrderItemMovement : ItemMovement
{
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual Order? Order { get; set; }

    public OrderItemMovement(int itemId, int amount) : base(itemId, amount) { }

    public override bool Equals(object? obj)
    {
        if (obj is OrderItemMovement orderItemMovement) return orderItemMovement.ItemId == ItemId && orderItemMovement.Amount == Amount;
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(ItemId, Amount);
}
