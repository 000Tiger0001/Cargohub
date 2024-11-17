using Newtonsoft.Json;

public class OrderItemMovement
{
    public Guid Id { get; set; } = new Guid();

    [JsonProperty("item_id")]
    public string? ItemId { get; set; }

    [JsonProperty("amount")]
    public int Amount { get; set; }
}