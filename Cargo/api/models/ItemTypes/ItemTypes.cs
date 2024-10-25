public class ItemTypes
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ItemTypes(string name, string description, DateTime createdAt, DateTime updatedAt)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}