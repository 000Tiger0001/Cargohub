using Xunit;
using Microsoft.EntityFrameworkCore;

public class TransferItemMovementTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private readonly TransferItemMovementAccess _TransferItemMovementAccess;
    private readonly TransferItemMovementServices _service;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _serviceItems;
    private readonly ItemGroupAccess _itemGroupAccess;
    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemTypeAccess _itemTypeAccess;
    private readonly ItemGroupServices _serviceItemGroup;
    private readonly ItemLineServices _serviceItemLine;
    private readonly ItemTypeServices _serviceItemType;
    private readonly SupplierAccess _supplierAccess;
    private readonly SupplierServices _serviceSupplier;
    private readonly WarehouseAccess _warehouseAccess;
    private readonly UserAccess _userAccess;
    private readonly InventoryServices _inventoryServices;
    private readonly LocationAccess _locationAccess;
    private LocationServices _locationServices;
    private InventoryAccess _inventoryAccess;

    public TransferItemMovementTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new(options);
        _userAccess = new(_dbContext);
        _warehouseAccess = new(_dbContext);
        _locationAccess = new(_dbContext);
        _inventoryAccess = new(_dbContext);
        _orderItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _TransferItemMovementAccess = new(_dbContext);
        _service = new(_TransferItemMovementAccess);
        _itemAccess = new(_dbContext);
        _supplierAccess = new(_dbContext);
        _serviceSupplier = new(_supplierAccess, _itemAccess);
        _itemGroupAccess = new(_dbContext);
        _itemLineAccess = new(_dbContext);
        _itemTypeAccess = new(_dbContext);
        _serviceItemGroup = new(_itemGroupAccess, _itemAccess);
        _serviceItemLine = new(_itemLineAccess, _itemAccess);
        _serviceItemType = new(_itemTypeAccess, _itemAccess);
        _locationServices = new(_locationAccess, _warehouseAccess, _inventoryAccess, _userAccess);
        _inventoryServices = new(_inventoryAccess, _locationAccess, _itemAccess, _userAccess, _locationServices);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _TransferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess, _supplierAccess, _inventoryServices);
    }
    [Fact]
    public async Task GetTransferItemMovements()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 0, 0, 0, 1, null!, "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);

        await _serviceItems.AddItem(item);

        Assert.Equal(await _serviceItems.GetItem(1), item);
        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 1, 2);
        TransferItemMovement sIM2 = new(2, 1, 3);

        await _service.AddTransferItemMovement(sIM1);
        await _service.AddTransferItemMovement(sIM2);

        Assert.Equal([sIM1, sIM2], await _service.GetTransferItemMovements());

        await _service.RemoveTransferItemMovement(1);
        await _service.RemoveTransferItemMovement(2);

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task GetTransferItemMovement()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 0, 0, 0, 1, null!, "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);

        await _serviceItems.AddItem(item);

        Assert.Equal(await _serviceItems.GetItem(1), item);
        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 1, 2);

        await _service.AddTransferItemMovement(sIM1);

        Assert.Equal(sIM1, await _service.GetTransferItemMovement(1));
        Assert.Null(await _service.GetTransferItemMovement(-1));
        Assert.Null(await _service.GetTransferItemMovement(2));

        await _service.RemoveTransferItemMovement(1);

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task AddTransferItemMovement()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 0, 0, 0, 1, null!, "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);

        await _serviceItems.AddItem(item);

        Assert.Equal(await _serviceItems.GetItem(1), item);
        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 1, 3);
        bool success = await _service.AddTransferItemMovement(sIM1);

        Assert.True(success);
        Assert.Equal([sIM1], await _service.GetTransferItemMovements());

        await _service.RemoveTransferItemMovement(1);

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task AddDuplicateId()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 0, 0, 0, 1, null!, "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item);

        Assert.Equal(await _serviceItems.GetItem(1), item);
        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 1, 3);

        await _service.AddTransferItemMovement(sIM1);

        Assert.Equal([sIM1], await _service.GetTransferItemMovements());

        TransferItemMovement sIM2 = new(1, 1, 2);

        bool success = await _service.AddTransferItemMovement(sIM2);

        Assert.False(success);
        Assert.Equal([sIM1], await _service.GetTransferItemMovements());

        await _service.RemoveTransferItemMovement(1);

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task RemoveTransferItemMovement()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 0, 0, 0, 1, null!, "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item);

        Assert.Equal(await _serviceItems.GetItem(1), item);
        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 1, 3);
        TransferItemMovement sIM2 = new(2, 1, 2);

        await _service.AddTransferItemMovement(sIM1);
        await _service.AddTransferItemMovement(sIM2);

        Assert.Equal([sIM1, sIM2], await _service.GetTransferItemMovements());

        bool success = await _service.RemoveTransferItemMovement(1);

        Assert.True(success);
        Assert.Equal([sIM2], await _service.GetTransferItemMovements());

        success = await _service.RemoveTransferItemMovement(3);

        Assert.False(success);
        Assert.Equal([sIM2], await _service.GetTransferItemMovements());

        await _service.RemoveTransferItemMovement(2);

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }


    [Fact]
    public async Task UpdateTransferItemMovement()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 0, 0, 0, 1, null!, "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);

        await _serviceItems.AddItem(item);

        Assert.Equal(await _serviceItems.GetItem(1), item);
        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 1, 2);

        await _service.AddTransferItemMovement(sIM1);

        Assert.Equal([sIM1], await _service.GetTransferItemMovements());

        TransferItemMovement sIM2 = new(1, 1, 3);

        bool success = await _service.UpdateTransferItemMovement(sIM2);

        Assert.True(success);
        Assert.Equal([sIM2], await _service.GetTransferItemMovements());

        await _service.RemoveTransferItemMovement(2);

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }
}