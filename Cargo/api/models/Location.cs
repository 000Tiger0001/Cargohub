public class Location : IHasId
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Location() { }
    
    public Location(Guid warehouseId, string code, string name)
    {
        Id = Guid.NewGuid();
        WarehouseId = warehouseId;
        Code = code;
        Name = name;
    }
}