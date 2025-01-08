using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Inventory : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("item_id")]
    public string? ItemIdString { get; set; }

    [JsonIgnore] // Ignore this property during serialization
    public int ItemId
    {
        get
        {
            if (!string.IsNullOrEmpty(ItemIdString) && ItemIdString.StartsWith("P"))
            {
                string numericPart = ItemIdString.Substring(1); // Remove the "P" prefix
                if (int.TryParse(numericPart, out int numericId)) return numericId;
            }
            return 0;
        }
        set
        {
            ItemIdString = "P" + value.ToString();
        }
    }

    [JsonIgnore]
    [ForeignKey("ItemId")]
    public virtual Item? Item { get; set; }

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

    public override bool Equals(object? obj)
    {
        if (obj is Inventory other)
        {
            return Id == other.Id &&
                   ItemId == other.ItemId &&
                   Description == other.Description &&
                   ItemReference == other.ItemReference &&
                   // Check Locations for null explicitly before using SequenceEqual
                   (Locations == null && other.Locations == null || Locations != null && other.Locations != null && Locations.SequenceEqual(other.Locations)) &&
                   TotalOnHand == other.TotalOnHand &&
                   TotalExpected == other.TotalExpected &&
                   TotalOrdered == other.TotalOrdered &&
                   TotalAllocated == other.TotalAllocated &&
                   TotalAvailable == other.TotalAvailable;
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}