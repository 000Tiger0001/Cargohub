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
}