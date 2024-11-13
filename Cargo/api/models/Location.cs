public class Location : IHasId
{
    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Location() { }
    
    public Location(int id, int warehouseId, string code, string name)
    {
        Id = id;
        WarehouseId = warehouseId;
        Code = code;
        Name = name;
    }
}