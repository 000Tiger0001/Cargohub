using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Location : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("warehouse_id")]
    public int WarehouseId { get; set; }

    [JsonIgnore]
    [ForeignKey("WarehouseId")]
    public virtual Warehouse? Warehouse { get; set; }

    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Location() { }

    public Location(int id, int warehouseId, string code, string name)
    {
        Id = id;
        WarehouseId = warehouseId;
        Code = code;
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Location location)
        {
            return location.Id == Id && location.WarehouseId == WarehouseId
            && location.Code == Code && location.Name == Name;
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}