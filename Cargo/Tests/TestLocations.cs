using Xunit;
using Microsoft.EntityFrameworkCore;

public class LocationTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly LocationAccess _locationAccess;
    private readonly LocationServices _locationService;
    private readonly WarehouseServices _warehouseService;

    public LocationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _locationAccess = new LocationAccess(_dbContext);
        _locationService = new(_locationAccess);
        _warehouseService = new(new(_dbContext));
    }

    [Fact]
    public async Task GetAllLocations()
    {
        Location mockLocation = new(1, 1, "A.1.0", "Row: A, Rack: 1, Shelf: 0");
        
        Assert.Empty(await _locationService.GetLocations());

        bool IsAdded = await _locationService.AddLocation(mockLocation);

        Assert.True(IsAdded);
        Assert.Equal([mockLocation], await _locationService.GetLocations());

        bool IsRemoved = await _locationService.RemoveLocation(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _locationService.GetLocations());
    }

    [Fact]
    public async Task GetLocation()
    {
        Location mockLocation = new(1, 1, "A.1.0", "Row: A, Rack: 1, Shelf: 0");

        await _locationService.AddLocation(mockLocation);

        Assert.Equal(mockLocation, await _locationService.GetLocation(1));
        Assert.Null(await _locationService.GetLocation(0));

        await _locationService.RemoveLocation(1);

        Assert.Null(await _locationService.GetLocation(1));
    }

    [Fact]
    public async Task GetLocationsInWarehouse()
    {
        Location mockLocation1 = new(1, 1, "A.1.0", "Row: A, Rack: 1, Shelf: 0");
        Location mockLocation2 = new(2, 1, "A.1.1", "Row: A, Rack: 1, Shelf: 1");
        Location mockLocation3 = new(3, 1, "A.1.2", "Row: A, Rack: 1, Shelf: 2");
        Warehouse mockWarehouse = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");

        Assert.Empty(await _locationService.GetLocationsInWarehouse(1));

        bool IsAdded1 = await _locationService.AddLocation(mockLocation1);
        bool IsAdded2 = await _locationService.AddLocation(mockLocation2);
        bool IsAdded3 = await _locationService.AddLocation(mockLocation3);
        bool IsAdded4 = await _warehouseService.AddWarehouse(mockWarehouse);

        Assert.True(IsAdded1);
        Assert.True(IsAdded2);
        Assert.True(IsAdded3);
        Assert.True(IsAdded4);
        Assert.Equal([mockLocation1, mockLocation2, mockLocation3], await _locationService.GetLocationsInWarehouse(1));

        bool IsRemoved1 = await _locationService.RemoveLocation(3);

        Assert.True(IsRemoved1);
        Assert.Equal([mockLocation1, mockLocation2], await _locationService.GetLocationsInWarehouse(1));

        bool IsRemoved2 = await _locationService.RemoveLocation(2);

        Assert.True(IsRemoved2);
        Assert.Equal([mockLocation1], await _locationService.GetLocationsInWarehouse(1));

        bool IsRemoved3 = await _locationService.RemoveLocation(1);

        Assert.True(IsRemoved3);
        Assert.Empty(await _locationService.GetLocationsInWarehouse(1));

        await _warehouseService.RemoveWarehouse(1);
    }
}