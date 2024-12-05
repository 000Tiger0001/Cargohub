using Xunit;
using Microsoft.EntityFrameworkCore;

public class WarehouseTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly WarehouseAccess _warehouseAccess;
    private readonly WarehouseServices _service;

    public WarehouseTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;

        _dbContext = new ApplicationDbContext(options);

        // Create a new instance of Access with the in-memory DbContext
        _warehouseAccess = new WarehouseAccess(_dbContext);

        // Create new instance of Service
        _service = new(_warehouseAccess);
    }

    [Fact]
    public async Task GetAllWarehouses()
    {
        Warehouse mockWarehouse = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        
        Assert.Empty(await _service.GetWarehouses());

        await _service.AddWarehouse(mockWarehouse);

        Assert.Equal([mockWarehouse], await _service.GetWarehouses());

        await _service.RemoveWarehouse(1);

        Assert.Empty(await _service.GetWarehouses());
    }
}