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

    [Fact]
    public async Task GetAllLocations()
    {
        Location mockLocation = new(1, 1, "A.1.0", "Row: A, Rack: 1, Shelf: 0");
        
        Assert.Empty(await _service.GetLocations());

        bool IsAdded = await _service.AddLocation(mockLocation);

        Assert.True(IsAdded);
        Assert.Equal([mockLocation], await _service.GetLocations());

        bool IsRemoved = await _service.RemoveLocation(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetLocations());
    }
}