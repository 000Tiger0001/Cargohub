using Newtonsoft.Json;

public class ItemLine : IHasId
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public ItemLine() { }

    public ItemLine(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}