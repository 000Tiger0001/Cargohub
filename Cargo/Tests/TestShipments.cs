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
    public async Task GetAllShipments()
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
    public async Task GetItemsInShipment()
    {
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, [new(7435, 23), new(9557, 1), new(9553, 50)]);
        List<ShipmentItemMovement> items = [new(7435, 23), new(9557, 1), new(9553, 50)];
        List<ShipmentItemMovement> wrongItems = [new(4533, 75), new(7546, 43), new(8633, 37)];

        await _service.AddShipment(mockShipment);

        Assert.Equal([mockShipment], await _service.GetShipments());
        Assert.Empty(await _service.GetItemsInShipment(2));
        Assert.NotEqual(wrongItems, await _service.GetItemsInShipment(1));
        Assert.Equal(items, await _service.GetItemsInShipment(1));
        Assert.Empty(await _service.GetItemsInShipment(0));
        Assert.Empty(await _service.GetItemsInShipment(-1));
        
        await _service.RemoveShipment(1);
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

    [Fact]
    public async Task AddShipmentWithDuplicateId()
    {
        Shipment mockShipment1 = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, [new(7435, 23), new(9557, 1), new(9553, 50)]);
        Shipment mockShipment2 = new(1, 2, 9, DateTime.Parse("1983-11-28"), DateTime.Parse("1983-11-30"), DateTime.Parse("1983-12-02"), 'I', "Transit", "Wit duur fijn vlieg.", "PostNL", "Royal Dutch Post and Parcel Service", "TwoDay", "Automatic", "Ground", 56, 42.25, [new(3790, 10), new(7369, 15), new(7311, 21)]);
        bool IsAdded1 = await _service.AddShipment(mockShipment1);

        Assert.True(IsAdded1);
        Assert.Equal([mockShipment1], await _service.GetShipments());

        bool IsAdded2 = await _service.AddShipment(mockShipment2);

        Assert.False(IsAdded2);
        Assert.Equal([mockShipment1], await _service.GetShipments());

        bool IsRemoved = await _service.RemoveShipment(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetShipments());
    }

    [Fact]
    public async Task UpdateShipment()
    {
        Shipment mockShipment1 = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, [new(7435, 23), new(9557, 1), new(9553, 50)]);
        Shipment mockShipment2 = new(1, 2, 9, DateTime.Parse("1983-11-28"), DateTime.Parse("1983-11-30"), DateTime.Parse("1983-12-02"), 'I', "Transit", "Wit duur fijn vlieg.", "PostNL", "Royal Dutch Post and Parcel Service", "TwoDay", "Automatic", "Ground", 56, 42.25, [new(3790, 10), new(7369, 15), new(7311, 21)]);

        bool IsAdded = await _service.AddShipment(mockShipment1);

        Assert.True(IsAdded);
        Assert.Equal([mockShipment1], await _service.GetShipments());

        bool IsUpdated = await _service.UpdateShipment(mockShipment2);

        Assert.True(IsUpdated);
        Assert.Equal([mockShipment2], await _service.GetShipments());
        Assert.NotEqual([mockShipment1], await _service.GetShipments());

        await _service.RemoveShipment(1);
    }

    [Fact]
    public async Task RemoveShipment()
    {
        Shipment mockShipment1 = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, [new(7435, 23), new(9557, 1), new(9553, 50)]);

        bool IsAdded = await _service.AddShipment(mockShipment1);

        Assert.True(IsAdded);
        Assert.Equal([mockShipment1], await _service.GetShipments());

        bool IsRemoved = await _service.RemoveShipment(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetShipments());
    }
}