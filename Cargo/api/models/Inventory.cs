using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Inventory : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int? Id { get; set; }

    [JsonProperty("item_id")]
    public int? ItemId { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("item_reference")]
    public string? ItemReference { get; set; }

    [JsonProperty("locations")]
    public List<int>? Locations { get; set; }

    [JsonProperty("total_on_hand")]
    public int TotalOnHand { get; set; }

    [JsonProperty("total_expected")]
    public int TotalExpected { get; set; }

    [JsonProperty("total_ordered")]
    public int TotalOrdered { get; set; }

    [JsonProperty("total_allocated")]
    public int TotalAllocated { get; set; }

    [JsonProperty("total_available")]
    public int TotalAvailable { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Inventory() { }

    public Inventory(int id, int itemId, string description, string itemReference, List<int> locations, int totalOnHand, int totalExpected, int totalOrdered, int totalAllocated, int totalAvailable)
    {
        Id = id;
        ItemId = itemId;
        Description = description;
        ItemReference = itemReference;
        Locations = locations;
        TotalOnHand = totalOnHand;
        TotalExpected = totalExpected;
        TotalOrdered = totalOrdered;
        TotalAllocated = totalAllocated;
        TotalAvailable = totalAvailable;
    }
}