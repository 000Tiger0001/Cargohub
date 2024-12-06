using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;

public class IntegrationTests
{
    private readonly ApplicationDbContext _dbContext;

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

        _dbContext = new ApplicationDbContext(options);

        // Create a new instance of LocationAccess with the in-memory DbContext
        _itemGroupAccess = new ItemGroupAccess(_dbContext);

        _itemAccess = new ItemAccess(_dbContext);

        // Create new instance of locationService
        _serviceItemGroup = new(_itemGroupAccess);
        _serviceItems = new(_itemAccess);

        // Initialize the controller with LocationAccess

        _itemLineAccess = new(_dbContext);
        _serviceItemLine = new(_itemLineAccess);

        _itemTypeAccess = new(_dbContext);
        _serviceItemType = new(_itemTypeAccess);

        _orderAccess = new(_dbContext);
        _serviceOrder = new(_orderAccess);

        _shipmentAccess = new(_dbContext);
        _serviceShipment = new(_shipmentAccess);

        _transferAccess = new(_dbContext);
        _serviceTransfer = new(_transferAccess);

        _warehouseAccess = new(_dbContext);
        _serviceWarehouse = new(_warehouseAccess);

        _locationAccess = new(_dbContext);
        _serviceLocation = new(_locationAccess);

        _supplierAccess = new(_dbContext);
        _serviceSupplier = new(_supplierAccess);
    }

    [Fact]
    public async Task ItemGroupDelete()
    {
        //Arrange
        ItemGroup testItemGroup = new(1, "Furniture", "");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 1, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");
        //Add the mock locations to the in-memory database
        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);
        bool IsItemAdded = await _serviceItems.AddItem(testItem);
        bool IsItemGroupRemoved = await _serviceItemGroup.RemoveItemGroup(1);

        //Act
        Item? result = await _serviceItems.GetItem(1);
        int? id = result!.ItemGroupId;

        //Assert
        Assert.True(IsItemGroupAdded);
        Assert.True(IsItemAdded);
        Assert.True(IsItemGroupRemoved);
        Assert.Equal(0, id);
    }

    [Fact]
    public async Task ItemLineDelete()
    {
        //Arrange
        ItemLine testItemLine = new(1, "Home Appliances", "");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");
        //Add the mock locations to the in-memory database
        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);
        bool IsItemAdded = await _serviceItems.AddItem(testItem);
        bool IsItemLineRemoved = await _serviceItemLine.RemoveItemLine(1);
        //Act
        Item? result = await _serviceItems.GetItem(1);
        int? id = result!.ItemLineId;

        //Assert
        Assert.True(IsItemLineAdded);
        Assert.True(IsItemAdded);
        Assert.True(IsItemLineRemoved);
        Assert.Equal(0, id);
    }

    [Fact]
    public async Task ItemTypeDelete()
    {
        //Arrange
        ItemType testItemType = new(1, "Desktop", "");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 1, 1, 47, 13, 11, 34, "SUP423", "E-86805-uTM"); ;
        //Add the mock locations to the in-memory database
        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);
        bool IsItemAdded = await _serviceItems.AddItem(testItem);
        bool IsItemTypeRemoved = await _serviceItemType.RemoveItemType(1);
        //Act
        Item? result = await _serviceItems.GetItem(1);
        var id = result!.ItemTypeId;

        //Assert
        Assert.True(IsItemTypeAdded);
        Assert.True(IsItemAdded);
        Assert.True(IsItemTypeRemoved);
        Assert.Equal(0, id);
    }

    [Fact]
    public async Task ItemDeleteOrder()
    {
        var testOrderItemMovement = new OrderItemMovement(1, 1);
        var testOrder = new Order { Id = 1 };
        var testItem = new Item { Id = 1 };
        await _dbContext.Orders.AddAsync(testOrder);
        await _dbContext.Items.AddAsync(testItem);
        await _serviceItems.RemoveItem(1);
        Assert.NotNull(_serviceItems.GetItem(1));
    }

    [Fact]
    public async Task ItemDeleteShipment()
    {
        var testShipmentItemMovement = new ShipmentItemMovement(1, 1);
        var testShipment = new Shipment { Id = 1 };
        var testItem = new Item { Id = 1 };
        await _dbContext.Shipments.AddAsync(testShipment);
        await _dbContext.Items.AddAsync(testItem);
        await _serviceItems.RemoveItem(1);
        Assert.NotNull(_serviceItems.GetItem(1));
    }

    [Fact]
    public async Task ItemDeleteTransfer()
    {
        var testTransferItemMovement = new TransferItemMovement(1, 1);
        var testTransfer = new Transfer { Id = 1 };
        var testItem = new Item { Id = 1 };
        await _dbContext.Transfers.AddAsync(testTransfer);
        await _dbContext.Items.AddAsync(testItem);
        await _serviceItems.RemoveItem(1);
        Assert.NotNull(_serviceItems.GetItem(1));
    }

    [Fact]
    public async Task WarehouseDeleteLocation()
    {
        var testWarehouse = new Warehouse { Id = 1 };
        var testLocation = new Location { WarehouseId = 1 };
        await _dbContext.Warehouses.AddAsync(testWarehouse);
        await _dbContext.Locations.AddAsync(testLocation);
        _serviceWarehouse.RemoveWarehouse(1);
        Location location = await _serviceLocation.GetLocation(1);
        Assert.Equal(0, location.WarehouseId);
    }

    [Fact]
    public async Task WarehouseDeleteOrder()
    {
        var testWarehouse = new Warehouse { Id = 1 };
        var testOrder = new Order { WarehouseId = 1 };
        await _dbContext.Warehouses.AddAsync(testWarehouse);
        await _dbContext.Orders.AddAsync(testOrder);
        _serviceWarehouse.RemoveWarehouse(1);
        Order order = await _serviceOrder.GetOrder(1);
        Assert.Null(order.WarehouseId);
    }

    [Fact]
    public async Task AddItemWithoutItemGroup()
    {
        var testItem = new Item { Id = 1, ItemGroupId = 1 };
        await _serviceItems.AddItem(testItem);
        Assert.Null(await _serviceItems.GetItem(1));
    }

    [Fact]
    public async Task AddItemWithoutItemLine()
    {
        var testItem = new Item { Id = 1, ItemLineId = 1 };
        await _serviceItems.AddItem(testItem);
        Assert.Null(await _serviceItems.GetItem(1));
    }

    [Fact]
    public async Task AddItemWithoutItemType()
    {
        var testItem = new Item { Id = 1, ItemTypeId = 1 };
        await _serviceItems.AddItem(testItem);
        Assert.Null(await _serviceItems.GetItem(1));
    }

    [Fact]
    public async Task AddShipmentWithoutOrder()
    {
        var testShipment = new Shipment { Id = 1, OrderId = 1 };
        await _serviceShipment.AddShipment(testShipment);
        Assert.Null(await _serviceShipment.GetShipment(1));
    }

    [Fact]
    public async Task AddOrderWithoutShipment()
    {
        var testOrder = new Order { Id = 1, ShipmentId = 1 };
        await _serviceOrder.AddOrder(testOrder);
        Assert.Null(await _serviceOrder.GetOrder(1));
    }

    [Fact]
    public async Task AddLocationWithoutWarehouse()
    {
        var testLocation = new Location { Id = 1, WarehouseId = 1 };
        await _serviceLocation.AddLocation(testLocation);
        Assert.Null(await _serviceLocation.GetLocation(1));
    }

    [Fact]
    public async Task RemoveSupplier()
    {
        var testSupplier = new Supplier { Id = 1, Code = "W" };
        var testItem = new Item { Id = 1, SupplierId = 1, SupplierCode = "W" };
        await _serviceSupplier.AddSupplier(testSupplier);
        await _serviceItems.AddItem(testItem);
        await _serviceSupplier.RemoveSupplier(1);
        Item result = await _serviceItems.GetItem(1);
        Assert.Null(result.Code);
        Assert.Null(result.SupplierId);
    }

    [Fact]
    public async Task AddItemWithoutSupplier()
    {
        var testItem = new Item { Id = 1, SupplierId = 1 };
        await _serviceItems.AddItem(testItem);
        Assert.Null(await _serviceItems.GetItem(1));
    }

    [Fact]
    public async Task AddShipmentWithoutItem()
    {
        ShipmentItemMovement testShipmentItemMovement = new ShipmentItemMovement(1, 3);
        Shipment testShipment = new Shipment { Id = 1, Items = { testShipmentItemMovement } };
        await _serviceShipment.AddShipment(testShipment);
        Assert.Null(await _serviceShipment.GetShipment(1));
    }

    [Fact]
    public async Task AddOrderWithoutItem()
    {
        OrderItemMovement testOrderItemMovement = new OrderItemMovement(1, 3);
        Order testOrder = new Order { Id = 1, Items = { testOrderItemMovement } };
        await _serviceOrder.AddOrder(testOrder);
        Assert.Null(await _serviceOrder.GetOrder(1));
    }

    [Fact]
    public async Task AddTransferWithoutItem()
    {
        TransferItemMovement testTransferItemMovement = new TransferItemMovement(1, 3);
        Transfer testTransfer = new Transfer { Id = 1, Items = [testTransferItemMovement] };
        await _serviceTransfer.AddTransfer(testTransfer);
        Assert.Null(await _serviceTransfer.GetTransfer(1));
    }
}
