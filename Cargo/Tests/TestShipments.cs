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

    [Fact]
    public async Task GetAllOrders()
    {
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, [new(7435, 23), new(9557, 1), new(9553, 50)]);
        
        Assert.Empty(await _service.GetShipments());

        await _service.AddShipment(mockShipment);

        Assert.Equal([mockShipment], await _service.GetShipments());

        await _service.RemoveShipment(1);

        Assert.Empty(await _service.GetShipments());
    }

    [Fact]
    public async Task GetOrder()
    {
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, [new(7435, 23), new(9557, 1), new(9553, 50)]);

        await _service.AddShipment(mockShipment);

        Assert.Equal(mockShipment, await _service.GetShipment(1));
        Assert.Null(await _service.GetShipment(0));

        await _service.RemoveShipment(1);

        Assert.Empty(await _service.GetShipments());
    }

    [Fact]
    public async Task AddShipmentGood()
    {
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, [new(7435, 23), new(9557, 1), new(9553, 50)]);

        bool IsAdded = await _service.AddShipment(mockShipment);

        Assert.True(IsAdded);
        Assert.Equal([mockShipment], await _service.GetShipments());

        bool IsRemoved = await _service.RemoveShipment(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetShipments());
    }

    [Fact]
    public async Task AddShipmentBad()
    {
        Client mockClient = new(1, "testName", "LOC1", "testCity", "1234AB", "testProvince", "testCountry", "testName", "testPhone", "testEmail");

        Assert.Empty(await _service.GetShipments());

        /* De code hieronder is uitgecomment, omdat het een error geeft. */
        //await _service.AddShipment(mockClient);

        Assert.Empty(await _service.GetShipments());

        await _service.RemoveShipment(1);

        Assert.Empty(await _service.GetShipments());
    }

    [Fact]
    public async Task AddDuplicateShipment()
    {
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, [new(7435, 23), new(9557, 1), new(9553, 50)]);

        bool IsAdded = await _service.AddShipment(mockShipment);

        Assert.True(IsAdded);
        Assert.Equal([mockShipment], await _service.GetShipments());

        bool IsAdded1 = await _service.AddShipment(mockShipment);

        Assert.True(IsAdded1);
        Assert.Equal([mockShipment], await _service.GetShipments());

        bool IsRemoved = await _service.RemoveShipment(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetShipments());
    }
}