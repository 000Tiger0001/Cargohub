using Xunit;
using Microsoft.EntityFrameworkCore;

public class OrderTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly OrderAccess _orderAccess;
    private readonly OrderServices _service;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _serviceItems;
    private readonly ClientAccess _clientAccess;
    private readonly ClientServices _serviceClients;
    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly TransferItemMovementAccess _transferItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;

    public OrderTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _orderAccess = new OrderAccess(_dbContext);
        _service = new(_orderAccess);
        _itemAccess = new(_dbContext);
        _orderItemMovementAccess = new(_dbContext);
        _transferItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _transferItemMovementAccess, _shipmentItemMovementAccess);
        _clientAccess = new(_dbContext);
        _serviceClients = new(_clientAccess);
    }

    [Fact]
    public async Task GetAllOrders()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, []);
        Order mockOrder2 = new(2, 9, DateTime.Parse("1999-07-05T19:31:10Z"), DateTime.Parse("1999-07-09T19:31:10Z"), "ORD00002", "Vergelijken raak geluid beetje altijd.", "Delivered", "We hobby thee compleet wiel fijn.", "Nood provincie hier.", "Borstelen dit verf suiker.", 20, 0, 0, 2, 8484.98, 214.52, 665.09, 42.12, []);

        Assert.Empty(await _service.GetOrders());

        await _service.AddOrder(mockOrder1);

        Assert.Equal([mockOrder1], await _service.GetOrders());

        await _service.AddOrder(mockOrder2);

        Assert.Equal([mockOrder1, mockOrder2], await _service.GetOrders());

        await _service.RemoveOrder(1);
        await _service.RemoveOrder(2);
    }

    [Fact]
    public async Task GetOrder()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, []);

        Assert.Empty(await _service.GetOrders());

        await _service.AddOrder(mockOrder1);

        Assert.Equal(mockOrder1, await _service.GetOrder(1));
        Assert.Null(await _service.GetOrder(0));

        await _service.RemoveOrder(1);

        Assert.Null(await _service.GetOrder(1));
    }

    [Fact]
    public async Task GetItemsInOrder()
    {
        OrderItemMovement mockItem1 = new(7435, 23);
        OrderItemMovement mockItem2 = new(9557, 1);
        OrderItemMovement mockItem3 = new(9553, 50);
        OrderItemMovement mockItem4 = new(10015, 16);
        OrderItemMovement mockItem5 = new(2084, 33);
        OrderItemMovement mockItem6 = new(3790, 10);
        OrderItemMovement mockItem7 = new(7369, 15);
        OrderItemMovement mockItem8 = new(7311, 21);
        OrderItemMovement mockItem9 = new(10669, 16);

        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 0, 0, 0, 100, 100, 100, 0, "0000", "0000");
        Item item2 = new(9557, "hdaffhhds2", "random2", "r2", "5555 EE2", "hoie2", "jooh2", 0, 0, 0, 100, 100, 100, 0, "0000", "0000");
        Item item3 = new(9553, "hdaffhhds3", "random3", "r3", "5555 EE3", "hoie3", "jooh3", 0, 0, 0, 100, 100, 100, 0, "0000", "0000");
        Item item4 = new(10015, "hdaffhhds4", "random4", "r4", "5555 EE4", "hoie4", "jooh4", 0, 0, 0, 100, 100, 100, 0, "0000", "0000");
        Item item5 = new(2084, "hdaffhhds5", "random5", "r5", "5555 EE5", "hoie5", "jooh5", 0, 0, 0, 100, 100, 100, 0, "0000", "0000");
        Item item6 = new(3790, "hdaffhhds6", "random6", "r6", "5555 EE6", "hoie6", "jooh6", 0, 0, 0, 100, 100, 100, 0, "0000", "0000");
        Item item7 = new(7369, "hdaffhhds7", "random7", "r7", "5555 EE7", "hoie7", "jooh7", 0, 0, 0, 100, 100, 100, 0, "0000", "0000");
        Item item8 = new(7311, "hdaffhhds8", "random8", "r8", "5555 EE8", "hoie8", "jooh8", 0, 0, 0, 100, 100, 100, 0, "0000", "0000");
        Item item9 = new(10669, "hdaffhhds9", "random9", "r9", "5555 EE9", "hoie9", "jooh9", 0, 0, 0, 100, 100, 100, 0, "0000", "0000");
        
        bool IsItemAdded1 = await _serviceItems.AddItem(item1);

        Assert.True(IsItemAdded1);
        Assert.Equal([item1], await _serviceItems.GetItems());

        bool IsItemAdded2 = await _serviceItems.AddItem(item2);

        Assert.True(IsItemAdded2);
        Assert.Equal([item1, item2], await _serviceItems.GetItems());

        bool IsItemAdded3 = await _serviceItems.AddItem(item3);

        Assert.True(IsItemAdded3);
        Assert.Equal([item1, item2, item3], await _serviceItems.GetItems());

        bool IsItemAdded4 = await _serviceItems.AddItem(item4);

        Assert.True(IsItemAdded4);
        Assert.Equal([item1, item2, item3, item4], await _serviceItems.GetItems());

        bool IsItemAdded5 = await _serviceItems.AddItem(item5);

        Assert.True(IsItemAdded5);
        Assert.Equal([item1, item2, item3, item4, item5], await _serviceItems.GetItems());

        bool IsItemAdded6 = await _serviceItems.AddItem(item6);

        Assert.True(IsItemAdded6);
        Assert.Equal([item1, item2, item3, item4, item5, item6], await _serviceItems.GetItems());

        bool IsItemAdded7 = await _serviceItems.AddItem(item7);

        Assert.True(IsItemAdded7);
        Assert.Equal([item1, item2, item3, item4, item5, item6, item7], await _serviceItems.GetItems());

        bool IsItemAdded8 = await _serviceItems.AddItem(item8);

        Assert.True(IsItemAdded8);
        Assert.Equal([item1, item2, item3, item4, item5, item6, item7, item8], await _serviceItems.GetItems());

        bool IsItemAdded9 = await _serviceItems.AddItem(item9);

        Assert.True(IsItemAdded9);
        Assert.Equal([item1, item2, item3, item4, item5, item6, item7, item8, item9], await _serviceItems.GetItems());

        List<OrderItemMovement> items = [mockItem1, mockItem2, mockItem3, mockItem4, mockItem5, mockItem6, mockItem7, mockItem8, mockItem9];
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, items);
        List<OrderItemMovement> wrongItems = [new(8352, 67), new(5326, 90), new(6534, 78), new(8780, 20), new(9809, 34)];

        await _service.AddOrder(mockOrder1);

        Assert.Equal([mockOrder1], await _service.GetOrders());
        Assert.Empty(await _service.GetItemsInOrder(2));
        Assert.NotEqual(wrongItems, await _service.GetItemsInOrder(1));
        Assert.Equal(items, await _service.GetItemsInOrder(1));
        Assert.Empty(await _service.GetItemsInOrder(0));
        Assert.Empty(await _service.GetItemsInOrder(-1));

        await _service.RemoveOrder(1);

        bool IsItemRemoved1 = await _serviceItems.RemoveItem(7435);

        Assert.True(IsItemRemoved1);
        Assert.Equal([item2, item3, item4, item5, item6, item7, item8, item9], await _serviceItems.GetItems());

        bool IsItemRemoved2 = await _serviceItems.RemoveItem(9557);

        Assert.True(IsItemRemoved2);
        Assert.Equal([item3, item4, item5, item6, item7, item8, item9], await _serviceItems.GetItems());

        bool IsItemRemoved3 = await _serviceItems.RemoveItem(9553);

        Assert.True(IsItemRemoved3);
        Assert.Equal([item4, item5, item6, item7, item8, item9], await _serviceItems.GetItems());

        bool IsItemRemoved4 = await _serviceItems.RemoveItem(10015);

        Assert.True(IsItemRemoved4);
        Assert.Equal([item5, item6, item7, item8, item9], await _serviceItems.GetItems());

        bool IsItemRemoved5 = await _serviceItems.RemoveItem(2084);

        Assert.True(IsItemRemoved5);
        Assert.Equal([item6, item7, item8, item9], await _serviceItems.GetItems());

        bool IsItemRemoved6 = await _serviceItems.RemoveItem(3790);

        Assert.True(IsItemRemoved6);
        Assert.Equal([item7, item8, item9], await _serviceItems.GetItems());

        bool IsItemRemoved7 = await _serviceItems.RemoveItem(7369);

        Assert.True(IsItemRemoved7);
        Assert.Equal([item8, item9], await _serviceItems.GetItems());

        bool IsItemRemoved8 = await _serviceItems.RemoveItem(7311);

        Assert.True(IsItemRemoved8);
        Assert.Equal([item9], await _serviceItems.GetItems());

        bool IsItemRemoved9 = await _serviceItems.RemoveItem(10669);

        Assert.True(IsItemRemoved9);
        Assert.Empty(await _serviceItems.GetItems());
    }

    [Fact]
    public async Task GetOrdersInShipment()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, []);
        Order mockOrder2 = new(2, 9, DateTime.Parse("1999-07-05T19:31:10Z"), DateTime.Parse("1999-07-09T19:31:10Z"), "ORD00002", "Vergelijken raak geluid beetje altijd.", "Delivered", "We hobby thee compleet wiel fijn.", "Nood provincie hier.", "Borstelen dit verf suiker.", 20, 0, 0, 1, 8484.98, 214.52, 665.09, 42.12, []);
        Order mockOrder3 = new(3, 52, DateTime.Parse("1983-09-26T19:06:08Z"), DateTime.Parse("1983-09-30T19:06:08Z"), "ORD00003", "Vergeven kamer goed enkele wiel tussen.", "Delivered", "Zeil hoeveel onze map sex ding.", "Ontvangen schoon voorzichtig instrument ster vijver kunnen raam.", "Grof geven politie suiker bodem zuid.", 11, 0, 0, 2, 1156.14, 420.45, 677.42, 86.03, []);

        bool IsAdded1 = await _service.AddOrder(mockOrder1);
        bool IsAdded2 = await _service.AddOrder(mockOrder2);
        bool IsAdded3 = await _service.AddOrder(mockOrder3);

        Assert.True(IsAdded1);
        Assert.True(IsAdded2);
        Assert.True(IsAdded3);

        Assert.Equal([mockOrder1, mockOrder2, mockOrder3], await _service.GetOrders());
        Assert.Equal([mockOrder1, mockOrder2], await _service.GetOrdersInShipment(1));
        Assert.Equal([mockOrder3], await _service.GetOrdersInShipment(2));

        Assert.Empty(await _service.GetOrdersInShipment(-1));
        Assert.Empty(await _service.GetOrdersInShipment(0));
        Assert.Empty(await _service.GetOrdersInShipment(3));

        bool IsRemoved1 = await _service.RemoveOrder(1);
        bool IsRemoved2 = await _service.RemoveOrder(2);
        bool IsRemoved3 = await _service.RemoveOrder(3);

        Assert.True(IsRemoved1);
        Assert.True(IsRemoved2);
        Assert.True(IsRemoved3);
        Assert.Empty(await _service.GetOrders());
    }

    [Fact]
    public async Task GetOrdersForClient()
    {
        Client mockClient1 = new(1, "testName1", "LOC1", "testCity1", "1234AB1", "testProvince1", "testCountry1", "testName1", "testPhone1", "testEmail1");
        Client mockClient2 = new(2, "testName2", "LOC2", "testCity2", "1234AB2", "testProvince2", "testCountry2", "testName2", "testPhone2", "testEmail2");

        bool IsClientAdded1 = await _serviceClients.AddClient(mockClient1);

        Assert.True(IsClientAdded1);
        Assert.Equal([mockClient1], await _serviceClients.GetClients());

        bool IsClientAdded2 = await _serviceClients.AddClient(mockClient2);

        Assert.True(IsClientAdded2);
        Assert.Equal([mockClient1, mockClient2], await _serviceClients.GetClients());

        Order mockOrder1 = new(1, 1, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [new(7435, 23), new(9557, 1), new(9553, 50), new(10015, 16), new(2084, 33)]);
        Order mockOrder2 = new(2, 1, DateTime.Parse("1999-07-05T19:31:10Z"), DateTime.Parse("1999-07-09T19:31:10Z"), "ORD00002", "Vergelijken raak geluid beetje altijd.", "Delivered", "We hobby thee compleet wiel fijn.", "Nood provincie hier.", "Borstelen dit verf suiker.", 20, 0, 0, 2, 8484.98, 214.52, 665.09, 42.12, [new(3790, 10), new(7369, 15), new(7311, 21)]);
        Order mockOrder3 = new(3, 2, DateTime.Parse("1983-09-26T19:06:08Z"), DateTime.Parse("1983-09-30T19:06:08Z"), "ORD00003", "Vergeven kamer goed enkele wiel tussen.", "Delivered", "Zeil hoeveel onze map sex ding.", "Ontvangen schoon voorzichtig instrument ster vijver kunnen raam.", "Grof geven politie suiker bodem zuid.", 11, 0, 0, 3, 1156.14, 420.45, 677.42, 86.03, [new(10669, 16)]);

        bool IsAdded1 = await _service.AddOrder(mockOrder1);
        bool IsAdded2 = await _service.AddOrder(mockOrder2);
        bool IsAdded3 = await _service.AddOrder(mockOrder3);

        Assert.True(IsAdded1);
        Assert.True(IsAdded2);
        Assert.True(IsAdded3);

        Assert.Equal([mockOrder1, mockOrder2, mockOrder3], await _service.GetOrders());
        Assert.Equal([mockOrder1, mockOrder2], await _service.GetOrdersForClient(1));
        Assert.Equal([mockOrder3], await _service.GetOrdersForClient(2));

        Assert.Empty(await _service.GetOrdersForClient(-1));
        Assert.Empty(await _service.GetOrdersForClient(0));
        Assert.Empty(await _service.GetOrdersForClient(3));

        bool IsRemoved1 = await _service.RemoveOrder(1);
        bool IsRemoved2 = await _service.RemoveOrder(2);
        bool IsRemoved3 = await _service.RemoveOrder(3);

        Assert.True(IsRemoved1);
        Assert.True(IsRemoved2);
        Assert.True(IsRemoved3);
        Assert.Empty(await _service.GetOrders());
    }

    [Fact]
    public async Task AddOrderGood()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, []);

        Assert.Empty(await _service.GetOrders());

        bool IsAdded = await _service.AddOrder(mockOrder1);

        Assert.True(IsAdded);
        Assert.Equal([mockOrder1], await _service.GetOrders());

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
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, []);

        bool IsAdded = await _service.AddOrder(mockOrder1);

        Assert.True(IsAdded);
        Assert.Equal([mockOrder1], await _service.GetOrders());

        bool IsAdded1 = await _service.AddOrder(mockOrder1);

        Assert.False(IsAdded1);
        Assert.Equal([mockOrder1], await _service.GetOrders());

        bool IsRemoved = await _service.RemoveOrder(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetOrders());
    }

    [Fact]
    public async Task AddOrderWithDuplicateId()
    {
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, []);
        Order mockOrder2 = new(1, 9, DateTime.Parse("1999-07-05T19:31:10Z"), DateTime.Parse("1999-07-09T19:31:10Z"), "ORD00002", "Vergelijken raak geluid beetje altijd.", "Delivered", "We hobby thee compleet wiel fijn.", "Nood provincie hier.", "Borstelen dit verf suiker.", 20, 0, 0, 2, 8484.98, 214.52, 665.09, 42.12, []);

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
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, []);
        Order mockOrder2 = new(1, 9, DateTime.Parse("1999-07-05T19:31:10Z"), DateTime.Parse("1999-07-09T19:31:10Z"), "ORD00002", "Vergelijken raak geluid beetje altijd.", "Delivered", "We hobby thee compleet wiel fijn.", "Nood provincie hier.", "Borstelen dit verf suiker.", 20, 0, 0, 2, 8484.98, 214.52, 665.09, 42.12, []);

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
        Order mockOrder1 = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, []);

        bool IsAdded = await _service.AddOrder(mockOrder1);

        Assert.True(IsAdded);
        Assert.Equal([mockOrder1], await _service.GetOrders());

        bool IsRemoved = await _service.RemoveOrder(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetOrders());
    }
}