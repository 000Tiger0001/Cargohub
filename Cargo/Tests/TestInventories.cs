using Xunit;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

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
    private readonly InventoryServices _inventoryServices;
    private readonly UserAccess _useraccess;

    public InventoryTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;

        _dbContext = new(options);

        // Create a new instance of Access with the in-memory DbContext
        _warehouseAccess = new(_dbContext);
        _useraccess = new(_dbContext);
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
        _serviceItemGroup = new(_itemGroupAccess, _itemAccess);
        _serviceItemLine = new(_itemLineAccess, _itemAccess);
        _serviceItemType = new(_itemTypeAccess, _itemAccess);
        _serviceLocation = new(_locationAccess, _warehouseAccess, _inventoryAccess, _useraccess);
        _inventoryServices = new(_inventoryAccess, _locationAccess, _itemAccess, _useraccess, _serviceLocation);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _transferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess, _supplierAccess, _inventoryServices);
        _serviceWarehouse = new(_warehouseAccess, _locationAccess, _orderAccess);

        // Create new instance of Service
        _service = new(_inventoryAccess, _locationAccess, _itemAccess, _useraccess, _serviceLocation);
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

        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceSupplier.AddSupplier(testSupplier);
        await _serviceItems.AddItem(testItem);
        await _serviceWarehouse.AddWarehouse(testWarehouse);
        await _serviceLocation.AddLocation(location1);
        await _serviceLocation.AddLocation(location2);

        Assert.Empty(await _service.GetInventories());

        await _service.AddInventory(mockInventory);

        Assert.Equal([mockInventory], await _service.GetInventories());

        await _service.RemoveInventory(1);

        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task GetInventory()
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

        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceSupplier.AddSupplier(testSupplier);
        await _serviceItems.AddItem(testItem);
        await _serviceWarehouse.AddWarehouse(testWarehouse);
        await _serviceLocation.AddLocation(location1);
        await _serviceLocation.AddLocation(location2);

        await _service.AddInventory(mockInventory);

        Assert.Equal(mockInventory, await _service.GetInventory(1));
        Assert.Null(await _service.GetInventory(0));

        await _service.RemoveInventory(1);

        Assert.Null(await _service.GetInventory(1));
    }

    [Fact]
    public async Task GetInventoriesForItems()
    {
        Warehouse testWarehouse1 = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        Warehouse testWarehouse2 = new(2, "YQZZNL562", "Heemskerk cargo hub2", "Karlijndreef 2812", "4002 AS2", "Heemskerk2", "Friesland2", "NL2", "Fem Keijzer2", "(078) 00133632", "blamore@example.net2");
        ItemGroup testItemGroup1 = new(1, "Furniture", "");
        ItemGroup testItemGroup2 = new(2, "Furniture2", "");
        ItemLine testItemLine1 = new(1, "Home Appliances", "");
        ItemLine testItemLine2 = new(2, "Home Appliances2", "");
        ItemType testItemType1 = new(1, "Desktop", "");
        ItemType testItemType2 = new(2, "Desktop2", "");
        Supplier testSupplier1 = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");
        Supplier testSupplier2 = new(2, "SUP0002", "Lee, Parks and Johnson2", "5989 Sullivan Drives2", "Apt. 9962", "Port Anitaburgh2", "916882", "Illinois2", "Czech Republic2", "Toni Barnett2", "363.541.7282x368252", "LPaJ-SUP0002");
        Item testItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 1, 47, 13, 11, 1, "SUP0001", "E-86805-uTM");
        Item testItem2 = new(2, "sjQ23408K2", "Face-to-face clear-thinking complexity2", "must2", "65235409471222", "63-OFFTq0T2", "oTo3042", 2, 2, 2, 47, 13, 11, 2, "SUP0002", "E-86805-uTM2");
        Location location1 = new(3211, 1, "65384", "a.1.1");
        Location location2 = new(24700, 1, "78934", "a.1.2");
        Location location3 = new(19800, 2, "65384", "a.1.1");
        Location location4 = new(23653, 2, "78934", "a.1.2");
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(2, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653], 194, 0, 139, 41, 55);

        await _serviceItemGroup.AddItemGroup(testItemGroup1);
        await _serviceItemGroup.AddItemGroup(testItemGroup2);
        await _serviceItemLine.AddItemLine(testItemLine1);
        await _serviceItemLine.AddItemLine(testItemLine2);
        await _serviceItemType.AddItemType(testItemType1);
        await _serviceItemType.AddItemType(testItemType2);
        await _serviceSupplier.AddSupplier(testSupplier1);
        await _serviceSupplier.AddSupplier(testSupplier2);
        await _serviceItems.AddItem(testItem1);
        await _serviceItems.AddItem(testItem2);
        await _serviceWarehouse.AddWarehouse(testWarehouse1);
        await _serviceWarehouse.AddWarehouse(testWarehouse2);
        await _serviceLocation.AddLocation(location1);
        await _serviceLocation.AddLocation(location2);
        await _serviceLocation.AddLocation(location3);
        await _serviceLocation.AddLocation(location4);
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
        Warehouse testWarehouse1 = new(1, "YQZZNL561", "Heemskerk cargo hub1", "Karlijndreef 2811", "4002 AS1", "Heemskerk1", "Friesland1", "NL1", "Fem Keijzer1", "(078) 00133631", "blamore@example.net1");
        Warehouse testWarehouse2 = new(2, "YQZZNL562", "Heemskerk cargo hub2", "Karlijndreef 2812", "4002 AS2", "Heemskerk2", "Friesland2", "NL2", "Fem Keijzer2", "(078) 00133632", "blamore@example.net2");
        Warehouse testWarehouse3 = new(3, "YQZZNL563", "Heemskerk cargo hub3", "Karlijndreef 2813", "4002 AS3", "Heemskerk3", "Friesland3", "NL3", "Fem Keijzer3", "(078) 00133633", "blamore@example.net3");
        ItemGroup testItemGroup1 = new(1, "Furniture1", "");
        ItemGroup testItemGroup2 = new(2, "Furniture2", "");
        ItemGroup testItemGroup3 = new(3, "Furniture3", "");
        ItemLine testItemLine1 = new(1, "Home Appliances1", "");
        ItemLine testItemLine2 = new(2, "Home Appliances2", "");
        ItemLine testItemLine3 = new(3, "Home Appliances3", "");
        ItemType testItemType1 = new(1, "Desktop1", "");
        ItemType testItemType2 = new(2, "Desktop2", "");
        ItemType testItemType3 = new(3, "Desktop3", "");
        Supplier testSupplier1 = new(1, "SUP0001", "Lee, Parks and Johnson1", "5989 Sullivan Drives1", "Apt. 9961", "Port Anitaburgh1", "916881", "Illinois1", "Czech Republic1", "Toni Barnett1", "363.541.7282x368251", "LPaJ-SUP0001");
        Supplier testSupplier2 = new(2, "SUP0002", "Lee, Parks and Johnson2", "5989 Sullivan Drives2", "Apt. 9962", "Port Anitaburgh2", "916882", "Illinois2", "Czech Republic2", "Toni Barnett2", "363.541.7282x368252", "LPaJ-SUP0002");
        Supplier testSupplier3 = new(3, "SUP0003", "Lee, Parks and Johnson3", "5989 Sullivan Drives3", "Apt. 9963", "Port Anitaburgh3", "916883", "Illinois3", "Czech Republic2", "Toni Barnett3", "363.541.7282x368253", "LPaJ-SUP0003");
        Item testItem1 = new(1, "sjQ23408K1", "Face-to-face clear-thinking complexity1", "must1", "65235409471221", "63-OFFTq0T1", "oTo3041", 1, 1, 1, 47, 13, 11, 1, "SUP0001", "E-86805-uTM1");
        Item testItem2 = new(2, "sjQ23408K2", "Face-to-face clear-thinking complexity2", "must2", "65235409471222", "63-OFFTq0T2", "oTo3042", 2, 2, 2, 47, 13, 11, 2, "SUP0002", "E-86805-uTM2");
        Item testItem3 = new(3, "sjQ23408K3", "Face-to-face clear-thinking complexity3", "must3", "65235409471223", "63-OFFTq0T3", "oTo3043", 3, 3, 3, 47, 13, 11, 3, "SUP0003", "E-86805-uTM3");
        Location location1 = new(3211, 1, "65384", "a.1.1");
        Location location2 = new(24700, 1, "78934", "a.1.2");
        Location location3 = new(19800, 2, "65384", "a.1.1");
        Location location4 = new(23653, 2, "78934", "a.1.2");
        Location location5 = new(5321, 3, "65384", "a.1.1");
        Location location6 = new(21960, 3, "78934", "a.1.2");
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(2, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653], 194, 0, 139, 0, 55);
        Inventory mockInventory3 = new(3, 3, "Cloned actuating artificial intelligence", "QVm03739H", [5321, 21960], 24, 0, 90, 68, -134);

        await _serviceItemGroup.AddItemGroup(testItemGroup1);
        await _serviceItemGroup.AddItemGroup(testItemGroup2);
        await _serviceItemGroup.AddItemGroup(testItemGroup3);
        await _serviceItemLine.AddItemLine(testItemLine1);
        await _serviceItemLine.AddItemLine(testItemLine2);
        await _serviceItemLine.AddItemLine(testItemLine3);
        await _serviceItemType.AddItemType(testItemType1);
        await _serviceItemType.AddItemType(testItemType2);
        await _serviceItemType.AddItemType(testItemType3);
        await _serviceSupplier.AddSupplier(testSupplier1);
        await _serviceSupplier.AddSupplier(testSupplier2);
        await _serviceSupplier.AddSupplier(testSupplier3);
        await _serviceItems.AddItem(testItem1);
        await _serviceItems.AddItem(testItem2);
        await _serviceItems.AddItem(testItem3);
        await _serviceWarehouse.AddWarehouse(testWarehouse1);
        await _serviceWarehouse.AddWarehouse(testWarehouse2);
        await _serviceWarehouse.AddWarehouse(testWarehouse3);
        await _serviceLocation.AddLocation(location1);
        await _serviceLocation.AddLocation(location2);
        await _serviceLocation.AddLocation(location3);
        await _serviceLocation.AddLocation(location4);
        await _serviceLocation.AddLocation(location5);
        await _serviceLocation.AddLocation(location6);
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
        Warehouse testWarehouse = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        ItemGroup testItemGroup = new(1, "Furniture", "");
        ItemLine testItemLine = new(1, "Home Appliances", "");
        ItemType testItemType = new(1, "Desktop", "");
        Supplier testSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 1, 47, 13, 11, 1, "SUP0001", "E-86805-uTM");
        Location location1 = new(3211, 1, "65384", "a.1.1");
        Location location2 = new(24700, 1, "78934", "a.1.2");
        Inventory mockInventory = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700], 262, 0, 80, 41, 141);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceSupplier.AddSupplier(testSupplier);
        await _serviceItems.AddItem(testItem);
        await _serviceWarehouse.AddWarehouse(testWarehouse);
        await _serviceLocation.AddLocation(location1);
        await _serviceLocation.AddLocation(location2);

        Assert.Empty(await _service.GetInventories());

        bool IsAdded = await _service.AddInventory(mockInventory);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory], await _service.GetInventories());

        await _service.RemoveInventory(1);

        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task AddDuplicateInventory()
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
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceSupplier.AddSupplier(testSupplier);
        await _serviceItems.AddItem(testItem);
        await _serviceWarehouse.AddWarehouse(testWarehouse);
        await _serviceLocation.AddLocation(location1);
        await _serviceLocation.AddLocation(location2);

        bool IsAdded = await _service.AddInventory(mockInventory);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory], await _service.GetInventories());

        bool IsAdded1 = await _service.AddInventory(mockInventory);

        Assert.False(IsAdded1);
        Assert.Equal([mockInventory], await _service.GetInventories());

        bool IsRemoved = await _service.RemoveInventory(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task AddInventoryWithDuplicateId()
    {
        Warehouse testWarehouse1 = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        Warehouse testWarehouse2 = new(2, "YQZZNL562", "Heemskerk cargo hub2", "Karlijndreef 2812", "4002 AS2", "Heemskerk2", "Friesland2", "NL2", "Fem Keijzer2", "(078) 00133632", "blamore@example.net2");
        ItemGroup testItemGroup1 = new(1, "Furniture", "");
        ItemGroup testItemGroup2 = new(2, "Furniture2", "");
        ItemLine testItemLine1 = new(1, "Home Appliances", "");
        ItemLine testItemLine2 = new(2, "Home Appliances2", "");
        ItemType testItemType1 = new(1, "Desktop", "");
        ItemType testItemType2 = new(2, "Desktop2", "");
        Supplier testSupplier1 = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");
        Supplier testSupplier2 = new(2, "SUP0002", "Lee, Parks and Johnson2", "5989 Sullivan Drives2", "Apt. 9962", "Port Anitaburgh2", "916882", "Illinois2", "Czech Republic2", "Toni Barnett2", "363.541.7282x368252", "LPaJ-SUP0002");
        Item testItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 1, 47, 13, 11, 1, "SUP0001", "E-86805-uTM");
        Item testItem2 = new(2, "sjQ23408K2", "Face-to-face clear-thinking complexity2", "must2", "65235409471222", "63-OFFTq0T2", "oTo3042", 2, 2, 2, 47, 13, 11, 2, "SUP0002", "E-86805-uTM2");
        Location location1 = new(3211, 1, "65384", "a.1.1");
        Location location2 = new(24700, 1, "78934", "a.1.2");
        Location location3 = new(19800, 2, "65384", "a.1.1");
        Location location4 = new(23653, 2, "78934", "a.1.2");
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(1, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653], 194, 0, 139, 41, 55);

        await _serviceItemGroup.AddItemGroup(testItemGroup1);
        await _serviceItemGroup.AddItemGroup(testItemGroup2);
        await _serviceItemLine.AddItemLine(testItemLine1);
        await _serviceItemLine.AddItemLine(testItemLine2);
        await _serviceItemType.AddItemType(testItemType1);
        await _serviceItemType.AddItemType(testItemType2);
        await _serviceSupplier.AddSupplier(testSupplier1);
        await _serviceSupplier.AddSupplier(testSupplier2);
        await _serviceItems.AddItem(testItem1);
        await _serviceItems.AddItem(testItem2);
        await _serviceWarehouse.AddWarehouse(testWarehouse1);
        await _serviceWarehouse.AddWarehouse(testWarehouse2);
        await _serviceLocation.AddLocation(location1);
        await _serviceLocation.AddLocation(location2);
        await _serviceLocation.AddLocation(location3);
        await _serviceLocation.AddLocation(location4);

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
        Warehouse testWarehouse1 = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        Warehouse testWarehouse2 = new(2, "YQZZNL562", "Heemskerk cargo hub2", "Karlijndreef 2812", "4002 AS2", "Heemskerk2", "Friesland2", "NL2", "Fem Keijzer2", "(078) 00133632", "blamore@example.net2");
        ItemGroup testItemGroup1 = new(1, "Furniture", "");
        ItemGroup testItemGroup2 = new(2, "Furniture2", "");
        ItemLine testItemLine1 = new(1, "Home Appliances", "");
        ItemLine testItemLine2 = new(2, "Home Appliances2", "");
        ItemType testItemType1 = new(1, "Desktop", "");
        ItemType testItemType2 = new(2, "Desktop2", "");
        Supplier testSupplier1 = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");
        Supplier testSupplier2 = new(2, "SUP0002", "Lee, Parks and Johnson2", "5989 Sullivan Drives2", "Apt. 9962", "Port Anitaburgh2", "916882", "Illinois2", "Czech Republic2", "Toni Barnett2", "363.541.7282x368252", "LPaJ-SUP0002");
        Item testItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 1, 47, 13, 11, 1, "SUP0001", "E-86805-uTM");
        Item testItem2 = new(2, "sjQ23408K2", "Face-to-face clear-thinking complexity2", "must2", "65235409471222", "63-OFFTq0T2", "oTo3042", 2, 2, 2, 47, 13, 11, 2, "SUP0002", "E-86805-uTM2");
        Location location1 = new(3211, 1, "65384", "a.1.1");
        Location location2 = new(24700, 1, "78934", "a.1.2");
        Location location3 = new(19800, 2, "65384", "a.1.1");
        Location location4 = new(23653, 2, "78934", "a.1.2");
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(1, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653], 194, 0, 139, 41, 55);

        await _serviceItemGroup.AddItemGroup(testItemGroup1);
        await _serviceItemGroup.AddItemGroup(testItemGroup2);
        await _serviceItemLine.AddItemLine(testItemLine1);
        await _serviceItemLine.AddItemLine(testItemLine2);
        await _serviceItemType.AddItemType(testItemType1);
        await _serviceItemType.AddItemType(testItemType2);
        await _serviceSupplier.AddSupplier(testSupplier1);
        await _serviceSupplier.AddSupplier(testSupplier2);
        await _serviceItems.AddItem(testItem1);
        await _serviceItems.AddItem(testItem2);
        await _serviceWarehouse.AddWarehouse(testWarehouse1);
        await _serviceWarehouse.AddWarehouse(testWarehouse2);
        await _serviceLocation.AddLocation(location1);
        await _serviceLocation.AddLocation(location2);
        await _serviceLocation.AddLocation(location3);
        await _serviceLocation.AddLocation(location4);

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
        Warehouse testWarehouse = new(1, "YQZZNL56", "Heemskerk cargo hub", "Karlijndreef 281", "4002 AS", "Heemskerk", "Friesland", "NL", "Fem Keijzer", "(078) 0013363", "blamore@example.net");
        ItemGroup testItemGroup = new(1, "Furniture", "");
        ItemLine testItemLine = new(1, "Home Appliances", "");
        ItemType testItemType = new(1, "Desktop", "");
        Supplier testSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");
        Item testItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 1, 1, 1, 47, 13, 11, 1, "SUP0001", "E-86805-uTM");
        Location location1 = new(3211, 1, "65384", "a.1.1");
        Location location2 = new(24700, 1, "78934", "a.1.2");
        Inventory mockInventory = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700], 262, 0, 80, 41, 141);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceSupplier.AddSupplier(testSupplier);
        await _serviceItems.AddItem(testItem);
        await _serviceWarehouse.AddWarehouse(testWarehouse);
        await _serviceLocation.AddLocation(location1);
        await _serviceLocation.AddLocation(location2);

        bool IsAdded = await _service.AddInventory(mockInventory);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory], await _service.GetInventories());

        bool IsRemoved = await _service.RemoveInventory(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetInventories());
    }
}