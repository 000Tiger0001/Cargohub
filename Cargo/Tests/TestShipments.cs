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
    private readonly SupplierAccess _supplierAccess;
    private readonly SupplierServices _serviceSupplier;
    private readonly InventoryAccess _inventoryAccess;
    private readonly WarehouseAccess _warehouseAccess;
    private readonly UserAccess _userAccess;
    private readonly InventoryServices _inventoryServices;
    private readonly LocationAccess _locationAccess;
    private LocationServices _locationServices;


    public ShipmentTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new(options);
        _userAccess = new(_dbContext);
        _warehouseAccess = new(_dbContext);
        _locationAccess = new(_dbContext);
        _inventoryAccess = new(_dbContext);
        _shipmentAccess = new(_dbContext);
        _orderItemMovementAccess = new(_dbContext);
        _transferItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _orderAccess = new(_dbContext);
        _itemAccess = new(_dbContext);
        _supplierAccess = new(_dbContext);
        _serviceSupplier = new(_supplierAccess, _itemAccess);
        _service = new(_shipmentAccess, _shipmentItemMovementAccess, _inventoryAccess, _itemAccess, _orderAccess);
        _itemGroupAccess = new(_dbContext);
        _itemLineAccess = new(_dbContext);
        _itemTypeAccess = new(_dbContext);
        _serviceItemGroup = new(_itemGroupAccess, _itemAccess);
        _serviceItemLine = new(_itemLineAccess, _itemAccess);
        _serviceItemType = new(_itemTypeAccess, _itemAccess);
        _serviceOrder = new(_orderAccess, _orderItemMovementAccess, _inventoryAccess, _itemAccess, _userAccess);
        _locationServices = new(_locationAccess, _warehouseAccess, _inventoryAccess, _userAccess);
        _inventoryServices = new(_inventoryAccess, _locationAccess, _itemAccess, _userAccess, _locationServices);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _transferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess, _supplierAccess, _inventoryServices);
    }

    [Fact]
    public async Task GetAllShipments()
    {
        OrderItemMovement orderItemMovement = new(7435, 1);
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item1);
        await _serviceOrder.AddOrder(testOrder);

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
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item1);
        await _serviceOrder.AddOrder(testOrder);

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

        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item2 = new(9557, "hdaffhhds2", "random2", "r2", "5555 EE2", "hoie2", "jooh2", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item3 = new(9553, "hdaffhhds3", "random3", "r3", "5555 EE3", "hoie3", "jooh3", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item1);
        await _serviceItems.AddItem(item2);
        await _serviceItems.AddItem(item3);

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

        await _serviceItems.RemoveItem(7435);
        await _serviceItems.RemoveItem(9557);
        await _serviceItems.RemoveItem(9553);
        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task AddShipmentGood()
    {
        OrderItemMovement orderItemMovement = new(7435, 1);
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item1);
        await _serviceOrder.AddOrder(testOrder);

        bool IsAdded = await _service.AddShipment(mockShipment);

        Assert.True(IsAdded);
        Assert.Equal([mockShipment], await _service.GetShipments());

        bool IsRemoved = await _service.RemoveShipment(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetShipments());
    }

    [Fact]
    public async Task AddDuplicateShipment()
    {
        OrderItemMovement orderItemMovement = new(7435, 1);
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item1);
        await _serviceOrder.AddOrder(testOrder);

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
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item2 = new(9557, "hdaffhhds2", "random2", "r2", "5555 EE2", "hoie2", "jooh2", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Order testOrder1 = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Order testOrder2 = new(2, 1, DateTime.Now, DateTime.Now, "456", "4", "P", "To deliver", "Don't be carefull", "Trow", 1, 1, 1, 1, 12, 12, 12, 12, [mockItem2]);
        Shipment mockShipment1 = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);
        Shipment mockShipment2 = new(1, 2, 9, DateTime.Parse("1983-11-28"), DateTime.Parse("1983-11-30"), DateTime.Parse("1983-12-02"), 'I', "Transit", "Wit duur fijn vlieg.", "PostNL", "Royal Dutch Post and Parcel Service", "TwoDay", "Automatic", "Ground", 56, 42.25, []);
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item1);
        await _serviceItems.AddItem(item2);
        await _serviceOrder.AddOrder(testOrder1);
        await _serviceOrder.AddOrder(testOrder2);

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
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Item item2 = new(9557, "hdaffhhds2", "random2", "r2", "5555 EE2", "hoie2", "jooh2", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Order testOrder1 = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Order testOrder2 = new(2, 1, DateTime.Now, DateTime.Now, "456", "4", "P", "To deliver", "Don't be carefull", "Trow", 1, 1, 1, 1, 12, 12, 12, 12, [mockItem2]);
        Shipment mockShipment1 = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);
        Shipment mockShipment2 = new(1, 2, 9, DateTime.Parse("1983-11-28"), DateTime.Parse("1983-11-30"), DateTime.Parse("1983-12-02"), 'I', "Transit", "Wit duur fijn vlieg.", "PostNL", "Royal Dutch Post and Parcel Service", "TwoDay", "Automatic", "Ground", 56, 42.25, []);
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item1);
        await _serviceItems.AddItem(item2);
        await _serviceOrder.AddOrder(testOrder1);
        await _serviceOrder.AddOrder(testOrder2);

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
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Shipment mockShipment1 = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 18, 594.42, []);
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item1);
        await _serviceOrder.AddOrder(testOrder);

        bool IsAdded = await _service.AddShipment(mockShipment1);

        Assert.True(IsAdded);
        Assert.Equal([mockShipment1], await _service.GetShipments());

        bool IsRemoved = await _service.RemoveShipment(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetShipments());
    }
}