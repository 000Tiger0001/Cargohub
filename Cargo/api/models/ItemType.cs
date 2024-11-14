public class ItemType : IHasId
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public ItemType() { }
    public ItemType(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}