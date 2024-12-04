using Xunit;
using Microsoft.EntityFrameworkCore;

public class OrderTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly OrderAccess _orderAccess;
    private readonly OrderServices _service;

    public OrderTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _orderAccess = new OrderAccess(_dbContext);
        _service = new(_orderAccess);
    }

    [Fact]
    public async Task GetAllOrders()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [new(7435, 23), new(9557, 1), new(9553, 50), new(10015, 16), new(2084, 33)]);
        Order mockOrder2 = new(2, 9, DateTime.Parse("1999-07-05T19:31:10Z"), DateTime.Parse("1999-07-09T19:31:10Z"), "ORD00002", "Vergelijken raak geluid beetje altijd.", "Delivered", "We hobby thee compleet wiel fijn.", "Nood provincie hier.", "Borstelen dit verf suiker.", 20, 0, 0, 2, 8484.98, 214.52, 665.09, 42.12, [new(3790, 10), new(7369, 15), new(7311, 21)]);

        Assert.Empty(await _service.GetOrders());

        await _service.AddOrder(mockOrder1);

        Assert.Equal([mockOrder1], await _service.GetOrders()); // This test fails, because the items in this order don't exist. This test isn't supposed to fail

        await _service.AddOrder(mockOrder2);

        Assert.Equal([mockOrder1, mockOrder2], await _service.GetOrders());

        await _service.RemoveOrder(1);
        await _service.RemoveOrder(2);
    }

    [Fact]
    public async Task GetOrder()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [new(7435, 23), new(9557, 1), new(9553, 50), new(10015, 16), new(2084, 33)]);

        Assert.Empty(await _service.GetOrders());

        await _service.AddOrder(mockOrder1);

        Assert.Equal(mockOrder1, await _service.GetOrder(1)); // This test fails, because the items in this order don't exist. This test isn't supposed to fail
        Assert.Null(await _service.GetOrder(0));

        await _service.RemoveOrder(1);

        Assert.Null(await _service.GetOrder(1));
    }

    [Fact]
    public async Task GetItemsInOrder()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [new(7435, 23), new(9557, 1), new(9553, 50), new(10015, 16), new(2084, 33)]);
        List<OrderItemMovement> items = [new(7435, 23), new(9557, 1), new(9553, 50), new(10015, 16), new(2084, 33)];
        List<OrderItemMovement> wrongItems = [new(8352, 67), new(5326, 90), new(6534, 78), new(8780, 20), new(9809, 34)];

        await _service.AddOrder(mockOrder1);

        Assert.Equal([mockOrder1], await _service.GetOrders());
        Assert.Empty(await _service.GetItemsInOrder(2));
        Assert.NotEqual(wrongItems, await _service.GetItemsInOrder(1));
        Assert.Equal(items, await _service.GetItemsInOrder(1));
        Assert.Empty(await _service.GetItemsInOrder(0));
        Assert.Empty(await _service.GetItemsInOrder(-1));

        await _service.RemoveOrder(1);
    }

    [Fact]
    public async Task AddOrderGood()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [new(7435, 23), new(9557, 1), new(9553, 50), new(10015, 16), new(2084, 33)]);

        Assert.Empty(await _service.GetOrders());

        bool IsAdded = await _service.AddOrder(mockOrder1);

        Assert.True(IsAdded);
        Assert.Equal([mockOrder1], await _service.GetOrders()); // This test fails, because the items in this order don't exist. This test isn't supposed to fail

        bool IsRemoved = await _service.RemoveOrder(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetOrders());
    }

    [Fact]
    public async Task AddOrderBad()
    {
        Client mockClient = new(1, "testName", "LOC1", "testCity", "1234AB", "testProvince", "testCountry", "testName", "testPhone", "testEmail");

        Assert.Empty(await _service.GetOrders());

        /* De code hieronder is uitgecomment, omdat het een error geeft. */
        //await _service.AddOrder(mockClient);

        Assert.Empty(await _service.GetOrders());

        await _service.RemoveOrder(1);

        Assert.Empty(await _service.GetOrders());
    }

    [Fact]
    public async Task AddDuplicateOrder()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [new(7435, 23), new(9557, 1), new(9553, 50), new(10015, 16), new(2084, 33)]);

        bool IsAdded = await _service.AddOrder(mockOrder1);

        Assert.True(IsAdded);
        Assert.Equal([mockOrder1], await _service.GetOrders()); // This test fails, because the items in this order don't exist. This test isn't supposed to fail

        bool IsAdded1 = await _service.AddOrder(mockOrder1);

        Assert.False(IsAdded1);
        Assert.Equal([mockOrder1], await _service.GetOrders()); // This test fails, because the items in this order don't exist. This test isn't supposed to fail

        bool IsRemoved = await _service.RemoveOrder(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetOrders());
    }

    [Fact]
    public async Task AddOrderWithDuplicateId()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [new(7435, 23), new(9557, 1), new(9553, 50), new(10015, 16), new(2084, 33)]);
        Order mockOrder2 = new(1, 9, DateTime.Parse("1999-07-05T19:31:10Z"), DateTime.Parse("1999-07-09T19:31:10Z"), "ORD00002", "Vergelijken raak geluid beetje altijd.", "Delivered", "We hobby thee compleet wiel fijn.", "Nood provincie hier.", "Borstelen dit verf suiker.", 20, 0, 0, 2, 8484.98, 214.52, 665.09, 42.12, [new(3790, 10), new(7369, 15), new(7311, 21)]);

        bool IsAdded = await _service.AddOrder(mockOrder1);

        Assert.True(IsAdded);
        Assert.Equal([mockOrder1], await _service.GetOrders());

        bool IsAdded1 = await _service.AddOrder(mockOrder2);

        Assert.False(IsAdded1);
        Assert.Equal([mockOrder1], await _service.GetOrders());

        bool IsRemoved = await _service.RemoveOrder(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetOrders());
    }

    [Fact]
    public async Task UpdateOrder()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [new(7435, 23), new(9557, 1), new(9553, 50), new(10015, 16), new(2084, 33)]);
        Order mockOrder2 = new(1, 9, DateTime.Parse("1999-07-05T19:31:10Z"), DateTime.Parse("1999-07-09T19:31:10Z"), "ORD00002", "Vergelijken raak geluid beetje altijd.", "Delivered", "We hobby thee compleet wiel fijn.", "Nood provincie hier.", "Borstelen dit verf suiker.", 20, 0, 0, 2, 8484.98, 214.52, 665.09, 42.12, [new(3790, 10), new(7369, 15), new(7311, 21)]);

        bool IsAdded = await _service.AddOrder(mockOrder1);

        Assert.True(IsAdded);
        Assert.Equal([mockOrder1], await _service.GetOrders());

        bool IsUpdated = await _service.UpdateOrder(mockOrder2);

        Assert.True(IsUpdated);
        Assert.Equal([mockOrder2], await _service.GetOrders());
        Assert.NotEqual([mockOrder1], await _service.GetOrders());

        await _service.RemoveOrder(1);
    }

    [Fact]
    public async Task RemoveOrder()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [new(7435, 23), new(9557, 1), new(9553, 50), new(10015, 16), new(2084, 33)]);

        bool IsAdded = await _service.AddOrder(mockOrder1);

        Assert.True(IsAdded);
        Assert.Equal([mockOrder1], await _service.GetOrders());

        bool IsRemoved = await _service.RemoveOrder(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetOrders());
    }
}