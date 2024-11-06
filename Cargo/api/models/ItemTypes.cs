public class ItemTypes
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public ItemTypes(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }
}