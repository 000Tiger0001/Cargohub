using Xunit;
using Microsoft.EntityFrameworkCore;

public class IntegrationTests
{
    private readonly ApplicationDbContext _dbContext;

    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private readonly TransferItemMovementAccess _transferItemMovementAccess;

    private readonly ItemGroupAccess _itemGroupAccess;
    private readonly ItemGroupServices _serviceItemGroup;

    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _serviceItems;

    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemLineServices _serviceItemLine;

    private readonly ItemTypeAccess _itemTypeAccess;
    private readonly ItemTypeServices _serviceItemType;

    private readonly OrderAccess _orderAccess;
    private readonly OrderServices _serviceOrder;

    private readonly ShipmentAccess _shipmentAccess;
    private readonly ShipmentServices _serviceShipment;


    private readonly TransferAccess _transferAccess;
    private readonly TransferServices _serviceTransfer;

    private readonly WarehouseAccess _warehouseAccess;
    private readonly WarehouseServices _serviceWarehouse;

    private readonly LocationAccess _locationAccess;
    private readonly LocationServices _serviceLocation;

    private readonly SupplierAccess _supplierAccess;
    private readonly SupplierServices _serviceSupplier;



    public IntegrationTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) //In-memory database
                        .Options;

        _dbContext = new(options);

        // Create a new instance of LocationAccess with the in-memory DbContext
        _itemGroupAccess = new(_dbContext);
        _itemAccess = new(_dbContext);

        _itemLineAccess = new(_dbContext);
        _serviceItemLine = new(_itemLineAccess, _itemAccess);

        _itemTypeAccess = new(_dbContext);
        _serviceItemType = new(_itemTypeAccess, _itemAccess);

        _orderItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _transferItemMovementAccess = new(_dbContext);
        _supplierAccess = new(_dbContext);

        // Create new instance of locationService
        _serviceItemGroup = new(_itemGroupAccess, _itemAccess);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _transferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess, _supplierAccess);

        // Initialize the controller with LocationAccess
        _shipmentAccess = new(_dbContext);

        _orderAccess = new(_dbContext);
        _serviceOrder = new(_orderAccess);

        _serviceShipment = new(_shipmentAccess, _itemAccess, _orderAccess);

        _transferAccess = new(_dbContext);
        _serviceTransfer = new(_transferAccess);

        _warehouseAccess = new(_dbContext);
        _locationAccess = new(_dbContext);
        _serviceLocation = new(_locationAccess, _warehouseAccess);
        _serviceWarehouse = new(_warehouseAccess, _locationAccess, _orderAccess);

        _serviceSupplier = new(_supplierAccess);
    }

    [Fact]
    public async Task ItemGroupDelete()
    {
        ItemGroup testItemGroup = new(1, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 1, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        bool IsSupplierAdded = await _serviceSupplier.AddSupplier(mockSupplier);

        Assert.True(IsSupplierAdded);
        Assert.Equal([mockSupplier], await _serviceSupplier.GetSuppliers());

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.True(IsItemAdded);
        Assert.Equal([testItem], await _serviceItems.GetItems());

        bool IsItemGroupRemoved = await _serviceItemGroup.RemoveItemGroup(1);

        Assert.True(IsItemGroupRemoved);
        Assert.Empty(await _serviceItemGroup.GetItemGroups());

        Item? result = await _serviceItems.GetItem(1);
        int? id = result!.ItemGroupId;

        Assert.Equal(0, id);
        Assert.NotEqual(testItem, result);

        bool IsItemRemoved = await _serviceItems.RemoveItem(1);

        Assert.True(IsItemRemoved);
        Assert.Empty(await _serviceItems.GetItems());

        bool IsItemLineRemoved = await _serviceItemLine.RemoveItemLine(11);

        Assert.True(IsItemLineRemoved);
        Assert.Empty(await _serviceItemLine.GetItemLines());

        bool IsItemTypeRemoved = await _serviceItemType.RemoveItemType(14);

        Assert.True(IsItemTypeRemoved);
        Assert.Empty(await _serviceItemType.GetItemTypes());
    }

    [Fact]
    public async Task ItemLineDelete()
    {
        ItemGroup testItemGroup = new(1, "Furniture", "");
        ItemLine testItemLine = new(1, "Home Appliances", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        bool IsSupplierAdded = await _serviceSupplier.AddSupplier(mockSupplier);

        Assert.True(IsSupplierAdded);
        Assert.Equal([mockSupplier], await _serviceSupplier.GetSuppliers());

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.True(IsItemAdded);
        Assert.Equal([testItem], await _serviceItems.GetItems());

        bool IsItemLineRemoved = await _serviceItemLine.RemoveItemLine(1);

        Assert.True(IsItemLineRemoved);
        Assert.Empty(await _serviceItemLine.GetItemLines());

        Item? result = await _serviceItems.GetItem(1);
        int? id = result!.ItemLineId;

        Assert.Equal(0, id);
        Assert.NotEqual(testItem, result);

        bool IsItemRemoved = await _serviceItems.RemoveItem(1);

        Assert.True(IsItemRemoved);
        Assert.Empty(await _serviceItems.GetItems());

        bool IsItemGroupRemoved = await _serviceItemGroup.RemoveItemGroup(1);

        Assert.True(IsItemGroupRemoved);
        Assert.Empty(await _serviceItemGroup.GetItemGroups());

        bool IsItemTypeRemoved = await _serviceItemType.RemoveItemType(14);

        Assert.True(IsItemTypeRemoved);
        Assert.Empty(await _serviceItemType.GetItemTypes());

    }

    [Fact]
    public async Task ItemTypeDelete()
    {
        ItemGroup testItemGroup = new(1, "Furniture", "");
        ItemLine testItemLine = new(11, "Home Appliances", "");
        ItemType testItemType = new(1, "Desktop", "");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 1, 1, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        bool IsSupplierAdded = await _serviceSupplier.AddSupplier(mockSupplier);

        Assert.True(IsSupplierAdded);
        Assert.Equal([mockSupplier], await _serviceSupplier.GetSuppliers());

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.True(IsItemAdded);
        Assert.Equal([testItem], await _serviceItems.GetItems());

        bool IsItemTypeRemoved = await _serviceItemType.RemoveItemType(1);

        Assert.True(IsItemTypeRemoved);
        Assert.Empty(await _serviceItemType.GetItemTypes());

        Item? result = await _serviceItems.GetItem(1);
        var id = result!.ItemTypeId;

        Assert.Equal(0, id);
        Assert.NotEqual(testItem, result);

        bool IsItemRemoved = await _serviceItems.RemoveItem(1);

        Assert.True(IsItemRemoved);
        Assert.Empty(await _serviceItems.GetItems());

        bool IsItemGroupRemoved = await _serviceItemGroup.RemoveItemGroup(1);

        Assert.True(IsItemGroupRemoved);
        Assert.Empty(await _serviceItemGroup.GetItemGroups());

        bool IsItemLineRemoved = await _serviceItemLine.RemoveItemLine(11);

        Assert.True(IsItemLineRemoved);
        Assert.Empty(await _serviceItemLine.GetItemLines());
    }

    [Fact]
    public async Task ItemDeleteOrder()
    {
        ItemGroup testItemGroup = new(1, "Furniture", "");
        ItemLine testItemLine = new(11, "Home Appliances", "");
        ItemType testItemType = new(1, "Desktop", "");
        OrderItemMovement testOrderItemMovement = new(1, 1);
        Order testOrder = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 18, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [testOrderItemMovement]);
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 1, 1, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        bool IsSupplierAdded = await _serviceSupplier.AddSupplier(mockSupplier);

        Assert.True(IsSupplierAdded);
        Assert.Equal([mockSupplier], await _serviceSupplier.GetSuppliers());

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.True(IsOrderAdded);
        Assert.Equal([testOrder], await _serviceOrder.GetOrders());

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.True(IsItemAdded);
        Assert.Equal([testItem], await _serviceItems.GetItems());

        bool IsItemRemoved = await _serviceItems.RemoveItem(1);

        Assert.True(IsItemRemoved);
        Assert.Empty(await _serviceItems.GetItems());
        Assert.Null(await _serviceItems.GetItem(1));

        Order? order = await _serviceOrder.GetOrder(1);
        Assert.Empty(order!.Items!);
        Assert.Empty(await _serviceOrder.GetItemsInOrder(1));

        bool IsOrderRemoved = await _serviceOrder.RemoveOrder(1);

        Assert.True(IsOrderRemoved);
        Assert.Empty(await _serviceOrder.GetOrders());

        bool IsItemGroupRemoved = await _serviceItemGroup.RemoveItemGroup(1);

        Assert.True(IsItemGroupRemoved);
        Assert.Empty(await _serviceItemGroup.GetItemGroups());

        bool IsItemLineRemoved = await _serviceItemLine.RemoveItemLine(11);

        Assert.True(IsItemLineRemoved);
        Assert.Empty(await _serviceItemLine.GetItemLines());

        bool IsItemTypeRemoved = await _serviceItemType.RemoveItemType(1);

        Assert.True(IsItemTypeRemoved);
        Assert.Empty(await _serviceItemType.GetItemTypes());
    }

    [Fact]
    public async Task ItemDeleteShipment()
    {
        ItemGroup testItemGroup = new(1, "Furniture", "");
        ItemLine testItemLine = new(11, "Home Appliances", "");
        ItemType testItemType = new(1, "Desktop", "");
        ShipmentItemMovement testShipmentItemMovement = new(1, 1);
        OrderItemMovement orderItemMovement = new(1, 1);
        Shipment testShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 31, 594.42, [testShipmentItemMovement]);
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 1, 1, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Order testOrder = new(1, 1, DateTime.Now, DateTime.Now, "123", "1", "P", "To deliver", "Be carefull", "Don't trow", 1, 1, 1, 1, 12, 12, 12, 12, [orderItemMovement]);
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        bool IsSupplierAdded = await _serviceSupplier.AddSupplier(mockSupplier);

        Assert.True(IsSupplierAdded);
        Assert.Equal([mockSupplier], await _serviceSupplier.GetSuppliers());

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.True(IsOrderAdded);
        Assert.Equal([testOrder], await _serviceOrder.GetOrders());

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.True(IsItemAdded);
        Assert.Equal([testItem], await _serviceItems.GetItems());

        bool IsShipmentAdded = await _serviceShipment.AddShipment(testShipment);

        Assert.True(IsShipmentAdded);
        Assert.Equal([testShipment], await _serviceShipment.GetShipments());

        bool IsItemRemoved = await _serviceItems.RemoveItem(1);

        Assert.True(IsItemRemoved);
        Assert.Empty(await _serviceItems.GetItems());
        Assert.Null(await _serviceItems.GetItem(1));

        Shipment? dbShipment = await _serviceShipment.GetShipment(1)!;

        Assert.Null(await _serviceItems.GetItem(1));
        Assert.Empty(dbShipment!.Items!);

        bool IsShipmentRemoved = await _serviceShipment.RemoveShipment(1);

        Assert.True(IsShipmentRemoved);
        Assert.Empty(await _serviceShipment.GetShipments());

        bool IsItemGroupRemoved = await _serviceItemGroup.RemoveItemGroup(1);

        Assert.True(IsItemGroupRemoved);
        Assert.Empty(await _serviceItemGroup.GetItemGroups());

        bool IsItemLineRemoved = await _serviceItemLine.RemoveItemLine(11);

        Assert.True(IsItemLineRemoved);
        Assert.Empty(await _serviceItemLine.GetItemLines());

        bool IsItemTypeRemoved = await _serviceItemType.RemoveItemType(1);

        Assert.True(IsItemTypeRemoved);
        Assert.Empty(await _serviceItemType.GetItemTypes());
    }

    [Fact]
    public async Task ItemDeleteTransfer()
    {
        ItemGroup testItemGroup = new(1, "Furniture", "");
        ItemLine testItemLine = new(11, "Home Appliances", "");
        ItemType testItemType = new(1, "Desktop", "");
        TransferItemMovement testTransferItemMovement = new(1, 23);
        Transfer testTransfer = new(1, "TR00001", 0, 9229, "Completed", [testTransferItemMovement]);
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 1, 1, 47, 13, 11, 1, "SUP423", "E-86805-uTM"); ;
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        bool IsSupplierAdded = await _serviceSupplier.AddSupplier(mockSupplier);

        Assert.True(IsSupplierAdded);
        Assert.Equal([mockSupplier], await _serviceSupplier.GetSuppliers());

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsTransferAdded = await _serviceTransfer.AddTransfer(testTransfer);

        Assert.True(IsTransferAdded);
        Assert.Equal([testTransfer], await _serviceTransfer.GetTransfers());

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.True(IsItemAdded);
        Assert.Equal([testItem], await _serviceItems.GetItems());

        bool IsItemRemoved = await _serviceItems.RemoveItem(1);

        Assert.True(IsItemRemoved);
        Assert.Empty(await _serviceItems.GetItems());
        Assert.Null(await _serviceItems.GetItem(1));
        Transfer? transfer = await _serviceTransfer.GetTransfer(1)!;
        Assert.Empty(transfer!.Items!);

        bool IsTransferRemoved = await _serviceTransfer.RemoveTransfer(1);

        Assert.True(IsTransferRemoved);
        Assert.Empty(await _serviceTransfer.GetTransfers());

        bool IsItemGroupRemoved = await _serviceItemGroup.RemoveItemGroup(1);

        Assert.True(IsItemGroupRemoved);
        Assert.Empty(await _serviceItemGroup.GetItemGroups());

        bool IsItemLineRemoved = await _serviceItemLine.RemoveItemLine(11);

        Assert.True(IsItemLineRemoved);
        Assert.Empty(await _serviceItemLine.GetItemLines());

        bool IsItemTypeRemoved = await _serviceItemType.RemoveItemType(1);

        Assert.True(IsItemTypeRemoved);
        Assert.Empty(await _serviceItemType.GetItemTypes());
    }

    [Fact]
    public async Task WarehouseDeleteLocation()
    {
        Warehouse testWarehouse = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        Location testLocation = new(1, 1, "A.1.0", "Row: A, Rack: 1, Shelf: 0");

        bool IsWarehouseAdded = await _serviceWarehouse.AddWarehouse(testWarehouse);

        Assert.True(IsWarehouseAdded);
        Assert.Equal([testWarehouse], await _serviceWarehouse.GetWarehouses());

        bool IsLocationAdded = await _serviceLocation.AddLocation(testLocation);

        Assert.True(IsLocationAdded);
        Assert.Equal([testLocation], await _serviceLocation.GetLocations());

        bool IsWarehouseRemoved = await _serviceWarehouse.RemoveWarehouse(1);

        Assert.True(IsWarehouseRemoved);
        Assert.Empty(await _serviceWarehouse.GetWarehouses());
        Assert.Null(await _serviceWarehouse.GetWarehouse(1));

        Location? location = await _serviceLocation.GetLocation(1);
        Assert.Equal(0, location!.WarehouseId);
        Assert.NotEqual(testLocation, location);

        bool IsLocationRemoved = await _serviceLocation.RemoveLocation(1);

        Assert.True(IsLocationRemoved);
        Assert.Empty(await _serviceLocation.GetLocations());
    }

    [Fact]
    public async Task WarehouseDeleteOrder()
    {
        Warehouse testWarehouse = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        Order testOrder = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 1, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, []);

        bool IsWarehouseAdded = await _serviceWarehouse.AddWarehouse(testWarehouse);

        Assert.True(IsWarehouseAdded);
        Assert.Equal([testWarehouse], await _serviceWarehouse.GetWarehouses());

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.True(IsOrderAdded);
        Assert.Equal([testOrder], await _serviceOrder.GetOrders());

        bool IsWarehouseRemoved = await _serviceWarehouse.RemoveWarehouse(1);

        Assert.True(IsWarehouseRemoved);
        Assert.Empty(await _serviceWarehouse.GetWarehouses());

        Order? order = await _serviceOrder.GetOrder(1);
        Assert.Equal(0, order!.WarehouseId);
        Assert.NotEqual(testOrder, order);

        bool IsOrderRemoved = await _serviceOrder.RemoveOrder(1);

        Assert.True(IsOrderRemoved);
        Assert.Empty(await _serviceOrder.GetOrders());
    }

    [Fact]
    public async Task AddItemWithoutItemGroup()
    {
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 1, 1, 47, 13, 11, 34, "SUP423", "E-86805-uTM"); ;

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.False(IsItemAdded);
        Assert.Empty(await _serviceItems.GetItems());
        Assert.Null(await _serviceItems.GetItem(1));
    }

    [Fact]
    public async Task AddItemWithoutItemLine()
    {
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 1, 47, 13, 11, 34, "SUP423", "E-86805-uTM"); ;

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.False(IsItemAdded);
        Assert.Empty(await _serviceItems.GetItems());
        Assert.Null(await _serviceItems.GetItem(1));
    }

    [Fact]
    public async Task AddItemWithoutItemType()
    {
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 1, 47, 13, 11, 34, "SUP423", "E-86805-uTM"); ;

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.False(IsItemAdded);
        Assert.Empty(await _serviceItems.GetItems());
        Assert.Null(await _serviceItems.GetItem(1));
    }

    [Fact]
    public async Task AddShipmentWithoutOrder()
    {
        ShipmentItemMovement testShipmentItemMovement = new(1, 3);
        Shipment testShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 31, 594.42, [testShipmentItemMovement]);

        bool IsShipmentAdded = await _serviceShipment.AddShipment(testShipment);

        Assert.False(IsShipmentAdded);
        Assert.Empty(await _serviceShipment.GetShipments());
        Assert.Null(await _serviceShipment.GetShipment(1));
    }

    /*[Fact]
    public async Task AddOrderWithoutShipment()
    {
        OrderItemMovement testOrderItemMovement = new(1, 3);
        Order testOrder = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 1, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [testOrderItemMovement]);

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.False(IsOrderAdded);
        Assert.Empty(await _serviceOrder.GetOrders());
        Assert.Null(await _serviceOrder.GetOrder(1));
    }*/

    [Fact]
    public async Task AddLocationWithoutWarehouse()
    {
        Location testLocation = new(1, 1, "A.1.0", "Row: A, Rack: 1, Shelf: 0");

        bool IsLocationAdded = await _serviceLocation.AddLocation(testLocation);

        Assert.False(IsLocationAdded);
        Assert.Empty(await _serviceLocation.GetLocations());
        Assert.Null(await _serviceLocation.GetLocation(1));
    }

    [Fact]
    public async Task RemoveSupplier()
    {
        Supplier testSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 1, 47, 13, 11, 1, "SUP0001", "E-86805-uTM");

        bool IsSupplierAdded = await _serviceSupplier.AddSupplier(testSupplier);

        Assert.True(IsSupplierAdded);
        Assert.Equal([testSupplier], await _serviceSupplier.GetSuppliers());

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.True(IsItemAdded);
        Assert.Equal([testItem], await _serviceItems.GetItems());

        bool IsSupplierRemoved = await _serviceSupplier.RemoveSupplier(1);

        Assert.True(IsSupplierRemoved);
        Assert.Empty(await _serviceSupplier.GetSuppliers());

        Item? result = await _serviceItems.GetItem(1);
        Supplier? supplierResult = await _serviceSupplier.GetSupplier(result!.SupplierId);
        Assert.Equal(0, result.SupplierId);
        Assert.NotEqual(testSupplier, supplierResult);
        Assert.Null(supplierResult);
    }

    [Fact]
    public async Task AddItemWithoutSupplier()
    {
        ItemGroup testItemGroup = new(1, "Furniture", "");
        ItemLine testItemLine = new(1, "Home Appliances", "");
        ItemType testItemType = new(1, "Desktop", "");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 1, 47, 13, 11, 34, "SUP423", "E-86805-uTM");

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.False(IsItemAdded);
        Assert.Empty(await _serviceItems.GetItems());
        Assert.Null(await _serviceItems.GetItem(1));
    }

    [Fact]
    public async Task AddShipmentWithoutItem()
    {
        ShipmentItemMovement testShipmentItemMovement = new(1, 3);
        Shipment testShipment = new(1, 1, 33, DateTime.Parse("2000-03-09"), DateTime.Parse("2000-03-11"), DateTime.Parse("2000-03-13"), 'I', "Pending", "Zee vertrouwen klas rots heet lachen oneven begrijpen.", "DPD", "Dynamic Parcel Distribution", "Fastest", "Manual", "Ground", 31, 594.42, [testShipmentItemMovement]);

        bool IsShipmentAdded = await _serviceShipment.AddShipment(testShipment);

        Assert.False(IsShipmentAdded);
        Assert.Empty(await _serviceShipment.GetShipments());
        Assert.Null(await _serviceShipment.GetShipment(1));
    }

    [Fact]
    public async Task AddOrderWithoutItem()
    {
        OrderItemMovement testOrderItemMovement = new(1, 3);
        Order testOrder = new(1, 33, DateTime.Parse("2019-04-03T11:33:15Z"), DateTime.Parse("2019-04-07T11:33:15Z"), "ORD00001", "Bedreven arm straffen bureau.", "Delivered", "Voedsel vijf vork heel.", "Buurman betalen plaats bewolkt.", "Ademen fijn volgorde scherp aardappel op leren.", 1, 0, 0, 1, 9905.13, 150.77, 372.72, 77.6, [testOrderItemMovement]);

        bool IsOrderAdded = await _serviceOrder.AddOrder(testOrder);

        Assert.False(IsOrderAdded);
        Assert.Empty(await _serviceOrder.GetOrders());
        Assert.Null(await _serviceOrder.GetOrder(1));
    }

    [Fact]
    public async Task AddTransferWithoutItem()
    {
        TransferItemMovement testTransferItemMovement = new(1, 3);
        Transfer testTransfer = new(1, "TR00001", 0, 9229, "Completed", [testTransferItemMovement]);

        bool IsTransferAdded = await _serviceTransfer.AddTransfer(testTransfer);

        Assert.False(IsTransferAdded);
        Assert.Empty(await _serviceTransfer.GetTransfers());
        Assert.Null(await _serviceTransfer.GetTransfer(1));
    }
}
