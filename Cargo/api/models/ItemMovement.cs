using Newtonsoft.Json;

public class ItemMovement
{
    public Guid Id { get; set; } = new Guid();

    [JsonProperty("item_id")]
    public string? ItemId { get; set; }

    [JsonProperty("amount")]
    public int Amount { get; set; }

    public ItemMovement() { }

    public ItemMovement(string itemId, int amount)
    {
        ItemId = itemId;
        Amount = amount;
    }
}