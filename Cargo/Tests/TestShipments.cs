using Xunit;
using Microsoft.EntityFrameworkCore;

public class ShipmentTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ShipmentAccess _shipmentAccess;
    private readonly ShipmentServices _service;

    public ShipmentTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _shipmentAccess = new ShipmentAccess(_dbContext);
        _service = new(_shipmentAccess);
    }
}