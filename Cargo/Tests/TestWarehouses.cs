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
}