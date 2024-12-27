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
    private readonly ItemGroupAccess _itemGroupAccess;
    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemTypeAccess _itemTypeAccess;
    private readonly ItemGroupServices _serviceItemGroup;
    private readonly ItemLineServices _serviceItemLine;
    private readonly ItemTypeServices _serviceItemType;
    private readonly ShipmentAccess _shipmentAccess;
    private readonly ShipmentServices _servicesShipment;
    private readonly SupplierAccess _supplierAccess;
    private readonly SupplierServices _serviceSupplier;
    private readonly InventoryAccess _inventoryAccess;
    private readonly UserAccess _userAccess;
    private readonly InventoryServices _inventoryServices;
    private readonly LocationAccess _locationAccess;
    private readonly LocationServices _locationServices;
    private readonly WarehouseAccess _warehouseAccess;

    public OrderTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new(options);
        _inventoryAccess = new(_dbContext);
        _orderAccess = new(_dbContext);
        _itemAccess = new(_dbContext);
        _supplierAccess = new(_dbContext);
        _serviceSupplier = new(_supplierAccess, _itemAccess);
        _orderItemMovementAccess = new(_dbContext);
        _transferItemMovementAccess = new(_dbContext);
        _locationAccess = new(_dbContext);
        _warehouseAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _itemGroupAccess = new(_dbContext);
        _userAccess = new(_dbContext);
        _itemLineAccess = new(_dbContext);
        _itemTypeAccess = new(_dbContext);
        _shipmentAccess = new(_dbContext);
        _servicesShipment = new(_shipmentAccess, _shipmentItemMovementAccess, _inventoryAccess, _itemAccess, _orderAccess);
        _locationServices = new(_locationAccess, _warehouseAccess, _inventoryAccess, _userAccess);
        _inventoryServices = new(_inventoryAccess, _locationAccess, _itemAccess, _userAccess, _locationServices);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _transferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess, _supplierAccess, _inventoryServices);
        _service = new(_orderAccess, _orderItemMovementAccess, _inventoryAccess, _itemAccess, _userAccess);
        _clientAccess = new(_dbContext);
        _serviceItemGroup = new(_itemGroupAccess, _itemAccess);
        _serviceItemLine = new(_itemLineAccess, _itemAccess);
        _serviceItemType = new(_itemTypeAccess, _itemAccess);
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
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");

        OrderItemMovement mockItem1 = new(7435, 23);
        OrderItemMovement mockItem2 = new(9557, 1);
        OrderItemMovement mockItem3 = new(9553, 50);
        OrderItemMovement mockItem4 = new(10015, 16);
        OrderItemMovement mockItem5 = new(2084, 33);
        OrderItemMovement mockItem6 = new(3790, 10);
        OrderItemMovement mockItem7 = new(7369, 15);
        OrderItemMovement mockItem8 = new(7311, 21);
        OrderItemMovement mockItem9 = new(10669, 16);

        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item2 = new(9557, "hdaffhhds2", "random2", "r2", "5555 EE2", "hoie2", "jooh2", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item3 = new(9553, "hdaffhhds3", "random3", "r3", "5555 EE3", "hoie3", "jooh3", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item4 = new(10015, "hdaffhhds4", "random4", "r4", "5555 EE4", "hoie4", "jooh4", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item5 = new(2084, "hdaffhhds5", "random5", "r5", "5555 EE5", "hoie5", "jooh5", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item6 = new(3790, "hdaffhhds6", "random6", "r6", "5555 EE6", "hoie6", "jooh6", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item7 = new(7369, "hdaffhhds7", "random7", "r7", "5555 EE7", "hoie7", "jooh7", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item8 = new(7311, "hdaffhhds8", "random8", "r8", "5555 EE8", "hoie8", "jooh8", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item9 = new(10669, "hdaffhhds9", "random9", "r9", "5555 EE9", "hoie9", "jooh9", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item1);
        await _serviceItems.AddItem(item2);
        await _serviceItems.AddItem(item3);
        await _serviceItems.AddItem(item4);
        await _serviceItems.AddItem(item5);
        await _serviceItems.AddItem(item6);
        await _serviceItems.AddItem(item7);
        await _serviceItems.AddItem(item8);
        await _serviceItems.AddItem(item9);

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

        await _serviceItems.RemoveItem(7435);
        await _serviceItems.RemoveItem(9557);
        await _serviceItems.RemoveItem(9553);
        await _serviceItems.RemoveItem(10015);
        await _serviceItems.RemoveItem(2084);
        await _serviceItems.RemoveItem(3790);
        await _serviceItems.RemoveItem(7369);
        await _serviceItems.RemoveItem(7311);
        await _serviceItems.RemoveItem(10669);
        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
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

        Order mockOrder1 = new(1, 1, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, []);
        Order mockOrder2 = new(2, 1, DateTime.Parse("1999-07-05T19:31:10Z"), DateTime.Parse("1999-07-09T19:31:10Z"), "ORD00002", "Vergelijken raak geluid beetje altijd.", "Delivered", "We hobby thee compleet wiel fijn.", "Nood provincie hier.", "Borstelen dit verf suiker.", 20, 0, 0, 2, 8484.98, 214.52, 665.09, 42.12, []);
        Order mockOrder3 = new(3, 2, DateTime.Parse("1983-09-26T19:06:08Z"), DateTime.Parse("1983-09-30T19:06:08Z"), "ORD00003", "Vergeven kamer goed enkele wiel tussen.", "Delivered", "Zeil hoeveel onze map sex ding.", "Ontvangen schoon voorzichtig instrument ster vijver kunnen raam.", "Grof geven politie suiker bodem zuid.", 11, 0, 0, 3, 1156.14, 420.45, 677.42, 86.03, []);

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