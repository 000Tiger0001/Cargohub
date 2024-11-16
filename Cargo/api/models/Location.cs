using Newtonsoft.Json;

public class Location : IHasId
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("warehouse_id")]
    public int WarehouseId { get; set; }

    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Location() { }

    public Location(string id, int warehouseId, string code, string name)
    {
        Id = id;
        WarehouseId = warehouseId;
        Code = code;
        Name = name;
    }
}