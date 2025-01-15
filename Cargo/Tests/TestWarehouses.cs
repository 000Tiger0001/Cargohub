using Xunit;
using Microsoft.EntityFrameworkCore;

public class WarehouseTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly OrderAccess _orderAccess;
    private readonly LocationAccess _locationAccess;
    private readonly WarehouseAccess _warehouseAccess;
    private readonly WarehouseServices _service;

    public WarehouseTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;

        _dbContext = new(options);

        // Create a new instance of Access with the in-memory DbContext
        _orderAccess = new(_dbContext);
        _warehouseAccess = new(_dbContext);
        _locationAccess = new(_dbContext);

        // Create new instance of Service
        _service = new(_warehouseAccess, _locationAccess, _orderAccess);
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

    [Fact]
    public async Task GetWarehouse()
    {
        Console.WriteLine("Imagine Some logic");
        Assert.True(1 + 1 == 2, "Showcase");
        Warehouse mockWarehouse = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");

        await _service.AddWarehouse(mockWarehouse);

        Assert.Equal(mockWarehouse, await _service.GetWarehouse(1));
        Assert.Null(await _service.GetWarehouse(0));

        await _service.RemoveWarehouse(1);

        Assert.Null(await _service.GetWarehouse(1));
    }

    [Fact]
    public async Task AddWarehouse()
    {
        Warehouse mockWarehouse = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");

        Assert.Empty(await _service.GetWarehouses());

        bool IsAdded = await _service.AddWarehouse(mockWarehouse);

        Assert.True(IsAdded);
        Assert.Equal([mockWarehouse], await _service.GetWarehouses());

        bool IsRemoved = await _service.RemoveWarehouse(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetWarehouses());
    }

    [Fact]
    public async Task AddDuplicateWarehouse()
    {
        Warehouse mockWarehouse = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");

        bool IsAdded1 = await _service.AddWarehouse(mockWarehouse);

        Assert.True(IsAdded1);
        Assert.Equal([mockWarehouse], await _service.GetWarehouses());

        bool IsAdded2 = await _service.AddWarehouse(mockWarehouse);

        Assert.False(IsAdded2);
        Assert.Equal([mockWarehouse], await _service.GetWarehouses());

        bool IsRemoved = await _service.RemoveWarehouse(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetWarehouses());
    }

    [Fact]
    public async Task AddWarehouseWithDuplicateId()
    {
        Warehouse mockWarehouse1 = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        Warehouse mockWarehouse2 = new(1, "GIOMNL90", "Petten longterm hub", "Owenweg 731", "4615 RB", "Petten", "Noord-Holland", "NL", "Maud Adryaens", "+31836 752702", "nickteunissen@example.com");

        bool IsAdded1 = await _service.AddWarehouse(mockWarehouse1);

        Assert.True(IsAdded1);
        Assert.Equal([mockWarehouse1], await _service.GetWarehouses());

        bool IsAdded2 = await _service.AddWarehouse(mockWarehouse2);

        Assert.False(IsAdded2);
        Assert.Equal([mockWarehouse1], await _service.GetWarehouses());

        bool IsRemoved = await _service.RemoveWarehouse(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetWarehouses());
    }

    [Fact]
    public async Task UpdateWarehouse()
    {
        Warehouse mockWarehouse1 = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        Warehouse mockWarehouse2 = new(1, "GIOMNL90", "Petten longterm hub", "Owenweg 731", "4615 RB", "Petten", "Noord-Holland", "NL", "Maud Adryaens", "+31836 752702", "nickteunissen@example.com");

        bool IsAdded = await _service.AddWarehouse(mockWarehouse1);

        Assert.True(IsAdded);
        Assert.Equal([mockWarehouse1], await _service.GetWarehouses());

        bool IsUpdated = await _service.UpdateWarehouse(mockWarehouse2);

        Assert.True(IsUpdated);
        Assert.Equal([mockWarehouse2], await _service.GetWarehouses());
        Assert.NotEqual([mockWarehouse1], await _service.GetWarehouses());

        await _service.RemoveWarehouse(1);
    }

    [Fact]
    public async Task RemoveWarehouse()
    {
        Warehouse mockWarehouse1 = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");

        bool IsAdded = await _service.AddWarehouse(mockWarehouse1);

        Assert.True(IsAdded);
        Assert.Equal([mockWarehouse1], await _service.GetWarehouses());

        bool IsRemoved = await _service.RemoveWarehouse(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetWarehouses());
    }
}