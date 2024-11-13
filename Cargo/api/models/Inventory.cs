public class Inventory : IHasId
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string Description { get; set; }
    public string ItemReference { get; set; }
    public List<Guid> Locations { get; set; }
    public int TotalOnHand { get; set; }
    public int TotalExpected { get; set; }
    public int TotalOrdered { get; set; }
    public int TotalAllocated { get; set; }
    public int TotalAvailable { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Inventory() { }

    public Inventory(Guid itemId, string description, string itemReference, List<Guid> locations, int totalOnHand, int totalExpected, int totalOrdered, int totalAllocated, int totalAvailable)
    {
        Id = Guid.NewGuid();
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