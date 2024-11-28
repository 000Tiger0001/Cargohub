using Xunit;
using Microsoft.EntityFrameworkCore;

public class LocationTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly LocationAccess _locationAccess;
    private readonly LocationServices _service;

    public LocationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _locationAccess = new LocationAccess(_dbContext);
        _service = new(_locationAccess);
    }
}