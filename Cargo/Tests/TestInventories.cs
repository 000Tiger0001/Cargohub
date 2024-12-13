using Xunit;
using Microsoft.EntityFrameworkCore;

public class InventoryTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly InventoryAccess _inventoryAccess;
    private readonly InventoryServices _service;
    private readonly LocationAccess _locationAccess;
    private readonly ItemAccess _itemAccess;
    private readonly LocationServices _serviceLocation;
    private readonly WarehouseAccess _warehouseAccess;
    private readonly WarehouseServices _serviceWarehouse;
    private readonly OrderAccess _orderAccess;
    private readonly ItemGroupAccess _itemGroupAccess;
    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemTypeAccess _itemTypeAccess;
    private readonly SupplierAccess _supplierAccess;
    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private readonly TransferItemMovementAccess _transferItemMovementAccess;
    private readonly SupplierServices _serviceSupplier;
    private readonly ItemServices _serviceItems;
    private readonly ItemGroupServices _serviceItemGroup;
    private readonly ItemLineServices _serviceItemLine;
    private readonly ItemTypeServices _serviceItemType;

    public InventoryTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;

        _dbContext = new(options);

        // Create a new instance of Access with the in-memory DbContext
        _warehouseAccess = new(_dbContext);
        _inventoryAccess = new(_dbContext);
        _locationAccess = new(_dbContext);
        _itemAccess = new(_dbContext);
        _orderAccess = new(_dbContext);
        _itemGroupAccess = new(_dbContext);
        _itemLineAccess = new(_dbContext);
        _itemTypeAccess = new(_dbContext);
        _supplierAccess = new(_dbContext);
        _orderItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _transferItemMovementAccess = new(_dbContext);
        _serviceSupplier = new(_supplierAccess, _itemAccess);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _transferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess, _supplierAccess);
        _serviceItemGroup = new(_itemGroupAccess, _itemAccess);
        _serviceItemLine = new(_itemLineAccess, _itemAccess);
        _serviceItemType = new(_itemTypeAccess, _itemAccess);
        _serviceLocation = new(_locationAccess, _warehouseAccess);
        _serviceWarehouse = new(_warehouseAccess, _locationAccess, _orderAccess);

        // Create new instance of Service
        _service = new(_inventoryAccess, _locationAccess, _itemAccess);
    }

    [Fact]
    public async Task GetAllInventories()
    {
        Warehouse testWarehouse = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        ItemGroup testItemGroup = new(1, "Furniture", "");
        ItemLine testItemLine = new(1, "Home Appliances", "");
        ItemType testItemType = new(1, "Desktop", "");
        Supplier testSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 1, 47, 13, 11, 1, "SUP0001", "E-86805-uTM");
        Location location1 = new(3211, 1, "65384", "a.1.1");
        Location location2 = new(24700, 1, "78934", "a.1.2");
        Inventory mockInventory = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700], 262, 0, 80, 41, 141);

        bool IsItemGroupAdded = await _serviceItemGroup.AddItemGroup(testItemGroup);

        Assert.True(IsItemGroupAdded);
        Assert.Equal([testItemGroup], await _serviceItemGroup.GetItemGroups());

        bool IsItemLineAdded = await _serviceItemLine.AddItemLine(testItemLine);

        Assert.True(IsItemLineAdded);
        Assert.Equal([testItemLine], await _serviceItemLine.GetItemLines());

        bool IsItemTypeAdded = await _serviceItemType.AddItemType(testItemType);

        Assert.True(IsItemTypeAdded);
        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());

        bool IsSupplierAdded = await _serviceSupplier.AddSupplier(testSupplier);

        Assert.True(IsSupplierAdded);
        Assert.Equal([testSupplier], await _serviceSupplier.GetSuppliers());

        bool IsItemAdded = await _serviceItems.AddItem(testItem);

        Assert.True(IsItemAdded);
        Assert.Equal([testItem], await _serviceItems.GetItems());

        bool IsWarehouseAdded = await _serviceWarehouse.AddWarehouse(testWarehouse);

        Assert.True(IsWarehouseAdded);
        Assert.Equal([testWarehouse], await _serviceWarehouse.GetWarehouses());

        bool IsLocationAdded1 = await _serviceLocation.AddLocation(location1);

        Assert.True(IsLocationAdded1);
        Assert.Equal([location1], await _serviceLocation.GetLocations());

        bool IsLocationAdded2 = await _serviceLocation.AddLocation(location2);

        Assert.True(IsLocationAdded2);
        Assert.Equal([location1, location2], await _serviceLocation.GetLocations());


        Assert.Empty(await _service.GetInventories());

        await _service.AddInventory(mockInventory);

        Assert.Equal([mockInventory], await _service.GetInventories());

        await _service.RemoveInventory(1);

        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task GetInventory()
    {
        Inventory mockInventory = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        await _service.AddInventory(mockInventory);

        Assert.Equal(mockInventory, await _service.GetInventory(1));
        Assert.Null(await _service.GetInventory(0));

        await _service.RemoveInventory(1);

        Assert.Null(await _service.GetInventory(1));
    }

    [Fact]
    public async Task GetInventoriesForItems()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(2, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653, 3068, 3334, 20477, 20524, 17579, 2271, 2293, 22717], 194, 0, 139, 41, 55);

        await _service.AddInventory(mockInventory1);
        await _service.AddInventory(mockInventory2);

        Assert.Equal([mockInventory1, mockInventory2], await _service.GetInventories());
        Assert.Equal([mockInventory1], await _service.GetInventoriesforItem(1));
        Assert.Equal([mockInventory2], await _service.GetInventoriesforItem(2));
        Assert.Empty(await _service.GetInventoriesforItem(3));

        bool IsRemoved1 = await _service.RemoveInventory(1);
        bool IsRemoved2 = await _service.RemoveInventory(2);

        Assert.True(IsRemoved1);
        Assert.True(IsRemoved2);
        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task GetInventoryTotalsForItems()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(2, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653, 3068, 3334, 20477, 20524, 17579, 2271, 2293, 22717], 194, 0, 139, 0, 55);
        Inventory mockInventory3 = new(3, 3, "Cloned actuating artificial intelligence", "QVm03739H", [5321, 21960], 24, 0, 90, 68, -134);

        bool IsAdded1 = await _service.AddInventory(mockInventory1);
        bool IsAdded2 = await _service.AddInventory(mockInventory2);
        bool IsAdded3 = await _service.AddInventory(mockInventory3);

        Assert.True(IsAdded1);
        Assert.True(IsAdded2);
        Assert.True(IsAdded3);
        Assert.Equal([mockInventory1, mockInventory2, mockInventory3], await _service.GetInventories());

        Dictionary<string, int> totals1 = await _service.GetInventoryTotalsForItem(mockInventory1.Id);
        Assert.Equal(mockInventory1.TotalExpected, totals1["total_expected"]);
        Assert.Equal(mockInventory1.TotalOrdered, totals1["total_ordered"]);
        Assert.Equal(mockInventory1.TotalAllocated, totals1["total_allocated"]);
        Assert.Equal(mockInventory1.TotalAvailable, totals1["total_available"]);

        Dictionary<string, int> totals2 = await _service.GetInventoryTotalsForItem(mockInventory2.Id);
        Assert.Equal(mockInventory2.TotalExpected, totals2["total_expected"]);
        Assert.Equal(mockInventory2.TotalOrdered, totals2["total_ordered"]);
        Assert.Equal(mockInventory2.TotalAllocated, totals2["total_allocated"]);
        Assert.Equal(mockInventory2.TotalAvailable, totals2["total_available"]);

        Dictionary<string, int> totals3 = await _service.GetInventoryTotalsForItem(mockInventory3.Id);
        Assert.Equal(mockInventory3.TotalExpected, totals3["total_expected"]);
        Assert.Equal(mockInventory3.TotalOrdered, totals3["total_ordered"]);
        Assert.Equal(mockInventory3.TotalAllocated, totals3["total_allocated"]);
        Assert.Equal(mockInventory3.TotalAvailable, totals3["total_available"]);

        bool IsRemoved1 = await _service.RemoveInventory(1);
        bool IsRemoved2 = await _service.RemoveInventory(2);
        bool IsRemoved3 = await _service.RemoveInventory(3);

        Assert.True(IsRemoved1);
        Assert.True(IsRemoved2);
        Assert.True(IsRemoved3);
        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task AddInventoryGood()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        Assert.Empty(await _service.GetInventories());

        bool IsAdded = await _service.AddInventory(mockInventory1);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        await _service.RemoveInventory(1);

        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task AddDuplicateInventory()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        bool IsAdded = await _service.AddInventory(mockInventory1);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsAdded1 = await _service.AddInventory(mockInventory1);

        Assert.False(IsAdded1);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsRemoved = await _service.RemoveInventory(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task AddInventoryWithDuplicateId()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(1, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653, 3068, 3334, 20477, 20524, 17579, 2271, 2293, 22717], 194, 0, 139, 0, 55);

        bool IsAdded = await _service.AddInventory(mockInventory1);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsAdded1 = await _service.AddInventory(mockInventory2);

        Assert.False(IsAdded1);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsRemoved = await _service.RemoveInventory(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task UpdateInventoryGood()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(1, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653, 3068, 3334, 20477, 20524, 17579, 2271, 2293, 22717], 194, 0, 139, 0, 55);

        bool IsAdded = await _service.AddInventory(mockInventory1);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsUpdated = await _service.UpdateInventory(mockInventory2);

        Assert.True(IsUpdated);
        Assert.Equal(mockInventory2, await _service.GetInventory(1));
        Assert.NotEqual(mockInventory1, await _service.GetInventory(1));

        await _service.RemoveInventory(1);
    }

    [Fact]
    public async Task RemoveInventory()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        bool IsAdded = await _service.AddInventory(mockInventory1);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsRemoved = await _service.RemoveInventory(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetInventories());
    }
}