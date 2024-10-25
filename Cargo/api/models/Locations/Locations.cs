public class Locations
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Locations(Guid warehouseId, string code, string name, DateTime createdAt, DateTime updatedAt)
    {
        Id = Guid.NewGuid();
        WarehouseId = warehouseId;
        Code = code;
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}