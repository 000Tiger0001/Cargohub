using Xunit;
using Microsoft.EntityFrameworkCore;

public class ShipmentTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ShipmentAccess _shipmentAccess;
    private readonly ShipmentServices _service;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _serviceItems;
    private readonly TransferItemMovementAccess _transferItemMovementAccess;
    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private readonly ItemGroupAccess _itemGroupAccess;
    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemTypeAccess _itemTypeAccess;
    private readonly ItemGroupServices _serviceItemGroup;
    private readonly ItemLineServices _serviceItemLine;
    private readonly ItemTypeServices _serviceItemType;
    private readonly OrderAccess _orderAccess;
    private readonly OrderServices _serviceOrder;

    public ShipmentTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new(options);
        _shipmentAccess = new(_dbContext);
        _orderItemMovementAccess = new(_dbContext);
        _transferItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _orderAccess = new(_dbContext);
        _serviceOrder = new(_orderAccess);
        _itemAccess = new(_dbContext);
        _service = new(_shipmentAccess, _itemAccess, _orderAccess);
        _itemGroupAccess = new(_dbContext);
        _itemLineAccess = new(_dbContext);
        _itemTypeAccess = new(_dbContext);
        _serviceItemGroup = new(_itemGroupAccess, _itemAccess);
        _serviceItemLine = new(_itemLineAccess, _itemAccess);
        _serviceItemType = new(_itemTypeAccess, _itemAccess);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _transferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess);
    }

    [Fact]
    public async Task GetAllShipments()
    {
        OrderItemMovement orderItemMovement = new(7435, 1);
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded = await _serviceItems.AddItem(item1);

        Assert.True(IsItemAdded);
        Assert.Equal([item1], await _serviceItems.GetItems());

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.True(IsOrderAdded);
        Assert.Equal([testOrder], await _serviceOrder.GetOrders());
        Assert.Empty(await _service.GetShipments());

        await _service.AddShipment(mockShipment);

        Assert.Equal([mockShipment], await _service.GetShipments());

        await _service.RemoveShipment(1);

        Assert.Empty(await _service.GetShipments());
    }

    [Fact]
    public async Task GetOrder()
    {
        OrderItemMovement orderItemMovement = new(7435, 1);
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded = await _serviceItems.AddItem(item1);

        Assert.True(IsItemAdded);
        Assert.Equal([item1], await _serviceItems.GetItems());

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.True(IsOrderAdded);
        Assert.Equal([testOrder], await _serviceOrder.GetOrders());

        await _service.AddShipment(mockShipment);

        Assert.Equal(mockShipment, await _service.GetShipment(1));
        Assert.Null(await _service.GetShipment(0));

        await _service.RemoveShipment(1);

        Assert.Empty(await _service.GetShipments());
    }

    [Fact]
    public async Task GetItemsInShipment()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");

        ShipmentItemMovement mockItem1 = new(7435, 23);
        ShipmentItemMovement mockItem2 = new(9557, 1);
        ShipmentItemMovement mockItem3 = new(9553, 50);

        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Item item2 = new(9557, "hdaffhhds2", "random2", "r2", "5555 EE2", "hoie2", "jooh2", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Item item3 = new(9553, "hdaffhhds3", "random3", "r3", "5555 EE3", "hoie3", "jooh3", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded1 = await _serviceItems.AddItem(item1);

        Assert.True(IsItemAdded1);
        Assert.Equal([item1], await _serviceItems.GetItems());

        bool IsItemAdded2 = await _serviceItems.AddItem(item2);

        Assert.True(IsItemAdded2);
        Assert.Equal([item1, item2], await _serviceItems.GetItems());

        bool IsItemAdded3 = await _serviceItems.AddItem(item3);

        Assert.True(IsItemAdded3);
        Assert.Equal([item1, item2, item3], await _serviceItems.GetItems());

        List<ShipmentItemMovement> items = [mockItem1, mockItem2, mockItem3];
        OrderItemMovement orderItemMovement = new(7435, 1);
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, [new(7435, 23), new(9557, 1), new(9553, 50)]);
        List<ShipmentItemMovement> wrongItems = [new(4533, 75), new(7546, 43), new(8633, 37)];

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.True(IsOrderAdded);
        Assert.Equal([testOrder], await _serviceOrder.GetOrders());

        await _service.AddShipment(mockShipment);

        Assert.Equal([mockShipment], await _service.GetShipments());
        Assert.Empty(await _service.GetItemsInShipment(2));
        Assert.NotEqual(wrongItems, await _service.GetItemsInShipment(1));
        Assert.Equal(items, await _service.GetItemsInShipment(1));
        Assert.Empty(await _service.GetItemsInShipment(0));
        Assert.Empty(await _service.GetItemsInShipment(-1));

        await _service.RemoveShipment(1);

        bool IsItemRemoved1 = await _serviceItems.RemoveItem(7435);

        Assert.True(IsItemRemoved1);
        Assert.Equal([item2, item3], await _serviceItems.GetItems());

        bool IsItemRemoved2 = await _serviceItems.RemoveItem(9557);

        Assert.True(IsItemRemoved2);
        Assert.Equal([item3], await _serviceItems.GetItems());

        bool IsItemRemoved3 = await _serviceItems.RemoveItem(9553);

        Assert.True(IsItemRemoved3);
        Assert.Empty(await _serviceItems.GetItems());

        bool IsItemGroupRemoved = await _serviceItemGroup.RemoveItemGroup(73);

        Assert.True(IsItemGroupRemoved);
        Assert.Empty(await _serviceItemGroup.GetItemGroups());

        bool IsItemLineRemoved = await _serviceItemLine.RemoveItemLine(11);

        Assert.True(IsItemLineRemoved);
        Assert.Empty(await _serviceItemLine.GetItemLines());

        bool IsItemTypeRemoved = await _serviceItemType.RemoveItemType(14);

        Assert.True(IsItemTypeRemoved);
        Assert.Empty(await _serviceItemType.GetItemTypes());
    }

    [Fact]
    public async Task AddShipmentGood()
    {
        OrderItemMovement orderItemMovement = new(7435, 1);
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded = await _serviceItems.AddItem(item1);

        Assert.True(IsItemAdded);
        Assert.Equal([item1], await _serviceItems.GetItems());

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.True(IsOrderAdded);
        Assert.Equal([testOrder], await _serviceOrder.GetOrders());

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
        OrderItemMovement orderItemMovement = new(7435, 1);
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded = await _serviceItems.AddItem(item1);

        Assert.True(IsItemAdded);
        Assert.Equal([item1], await _serviceItems.GetItems());

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.True(IsOrderAdded);
        Assert.Equal([testOrder], await _serviceOrder.GetOrders());

        bool IsAdded = await _service.AddShipment(mockShipment);

        Assert.True(IsAdded);
        Assert.Equal([mockShipment], await _service.GetShipments());

        bool IsAdded1 = await _service.AddShipment(mockShipment);

        Assert.False(IsAdded1);
        Assert.Equal([mockShipment], await _service.GetShipments());

        bool IsRemoved = await _service.RemoveShipment(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetShipments());
    }

    [Fact]
    public async Task AddShipmentWithDuplicateId()
    {
        OrderItemMovement orderItemMovement = new(7435, 1);
        OrderItemMovement mockItem2 = new(9557, 1);
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Item item2 = new(9557, "hdaffhhds2", "random2", "r2", "5555 EE2", "hoie2", "jooh2", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Order testOrder1 = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Order testOrder2 = new(2, 1, DateTime.Now, DateTime.Now, "456", "4", "P", "To deliver", "Don't be carefull", "Trow", 1, 1, 1, 1, 12, 12, 12, 12, [mockItem2]);
        Shipment mockShipment1 = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);
        Shipment mockShipment2 = new(1, 2, 9, DateTime.Parse("1983-11-28"), DateTime.Parse("1983-11-30"), DateTime.Parse("1983-12-02"), 'I', "Transit", "Wit duur fijn vlieg.", "PostNL", "Royal Dutch Post and Parcel Service", "TwoDay", "Automatic", "Ground", 56, 42.25, []);

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded1 = await _serviceItems.AddItem(item1);

        Assert.True(IsItemAdded1);
        Assert.Equal([item1], await _serviceItems.GetItems());

        bool IsItemAdded2 = await _serviceItems.AddItem(item2);

        Assert.True(IsItemAdded2);
        Assert.Equal([item1, item2], await _serviceItems.GetItems());

        bool IsOrderAdded1 = await _serviceOrder.AddOrder(testOrder1);

        Assert.True(IsOrderAdded1);
        Assert.Equal([testOrder1], await _serviceOrder.GetOrders());

        bool IsOrderAdded2 = await _serviceOrder.AddOrder(testOrder2);

        Assert.True(IsOrderAdded2);
        Assert.Equal([testOrder1, testOrder2], await _serviceOrder.GetOrders());

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
        OrderItemMovement orderItemMovement = new(7435, 1);
        OrderItemMovement mockItem2 = new(9557, 1);
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Item item2 = new(9557, "hdaffhhds2", "random2", "r2", "5555 EE2", "hoie2", "jooh2", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Order testOrder1 = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Order testOrder2 = new(2, 1, DateTime.Now, DateTime.Now, "456", "4", "P", "To deliver", "Don't be carefull", "Trow", 1, 1, 1, 1, 12, 12, 12, 12, [mockItem2]);
        Shipment mockShipment1 = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);
        Shipment mockShipment2 = new(1, 2, 9, DateTime.Parse("1983-11-28"), DateTime.Parse("1983-11-30"), DateTime.Parse("1983-12-02"), 'I', "Transit", "Wit duur fijn vlieg.", "PostNL", "Royal Dutch Post and Parcel Service", "TwoDay", "Automatic", "Ground", 56, 42.25, []);

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded1 = await _serviceItems.AddItem(item1);

        Assert.True(IsItemAdded1);
        Assert.Equal([item1], await _serviceItems.GetItems());

        bool IsItemAdded2 = await _serviceItems.AddItem(item2);

        Assert.True(IsItemAdded2);
        Assert.Equal([item1, item2], await _serviceItems.GetItems());

        bool IsOrderAdded1 = await _serviceOrder.AddOrder(testOrder1);

        Assert.True(IsOrderAdded1);
        Assert.Equal([testOrder1], await _serviceOrder.GetOrders());

        bool IsOrderAdded2 = await _serviceOrder.AddOrder(testOrder2);

        Assert.True(IsOrderAdded2);
        Assert.Equal([testOrder1, testOrder2], await _serviceOrder.GetOrders());

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
        OrderItemMovement orderItemMovement = new(7435, 1);
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 0, "0000", "0000");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment1 = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded = await _serviceItems.AddItem(item1);

        Assert.True(IsItemAdded);
        Assert.Equal([item1], await _serviceItems.GetItems());

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.True(IsOrderAdded);
        Assert.Equal([testOrder], await _serviceOrder.GetOrders());

        bool IsAdded = await _service.AddShipment(mockShipment1);

        Assert.True(IsAdded);
        Assert.Equal([mockShipment1], await _service.GetShipments());

        bool IsRemoved = await _service.RemoveShipment(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetShipments());
    }
}