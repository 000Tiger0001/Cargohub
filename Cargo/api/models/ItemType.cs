using Newtonsoft.Json;

public class ItemType : IHasId
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public ItemType() { }
    public ItemType(string id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}