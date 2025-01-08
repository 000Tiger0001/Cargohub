using Xunit;
using Microsoft.EntityFrameworkCore;

public class TransferTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly TransferItemMovementAccess _transferItemMovementAccess;
    private readonly TransferAccess _transferAccess;
    private readonly TransferServices _service;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _serviceItems;
    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
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

    public TransferTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;

        _dbContext = new(options);

        // Create a new instance of Access with the in-memory DbContext
        _transferAccess = new(_dbContext);
        _orderItemMovementAccess = new(_dbContext);
        _transferItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);

        _userAccess = new(_dbContext);
        _warehouseAccess = new(_dbContext);
        _locationAccess = new(_dbContext);
        _inventoryAccess = new(_dbContext);

        // Create new instance of Service
        _itemAccess = new(_dbContext);
        _service = new(_transferAccess, _transferItemMovementAccess, _itemAccess);
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
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _transferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess, _supplierAccess, _inventoryServices);
    }

    [Fact]
    public async Task GetAllTransfers()
    {
        Transfer mockTransfer = new(1, "TR00001", 0, 9229, "Uncompleted", []);

        Assert.Empty(await _service.GetTransfers());

        await _service.AddTransfer(mockTransfer);

        Assert.Equal([mockTransfer], await _service.GetTransfers());

        await _service.RemoveTransfer(1);

        Assert.Empty(await _service.GetTransfers());
    }

    [Fact]
    public async Task GetTransfer()
    {
        Transfer mockTransfer = new(1, "TR00001", 0, 9229, "Uncompleted", []);

        bool IsAdded = await _service.AddTransfer(mockTransfer);
        Assert.True(IsAdded);

        Assert.Equal(mockTransfer, await _service.GetTransfer(1));
        Assert.Null(await _service.GetTransfer(0));

        await _service.RemoveTransfer(1);

        Assert.Null(await _service.GetTransfer(1));
    }

    [Fact]
    public async Task GetItemsInTransfer()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        TransferItemMovement mockItem1 = new(7435, 23);
        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 11, 73, 14, 100, 100, 100, 1, "0000", "0000");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _serviceItems.AddItem(item1);

        List<TransferItemMovement> items = [mockItem1];
        Transfer mockTransfer = new(2, "TR00001", 0, 9229, "Uncompleted", items);

        await _service.AddTransfer(mockTransfer);

        List<TransferItemMovement>? transferItems = await _service.GetItemsInTransfer(2);
        int listSize = transferItems!.Count;

        Assert.Equal(items, transferItems);
        Assert.Equal(1, listSize);

        await _service.RemoveTransfer(2);

        Assert.Empty(await _service.GetTransfers());

        await _serviceItems.RemoveItem(7435);
        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task AddTransfer()
    {
        Transfer mockTransfer = new(1, "TR00001", 0, 9229, "Uncompleted", []);

        Assert.Empty(await _service.GetTransfers());

        bool IsAdded = await _service.AddTransfer(mockTransfer);

        Assert.True(IsAdded);
        Assert.Equal([mockTransfer], await _service.GetTransfers());

        bool IsRemoved = await _service.RemoveTransfer(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetTransfers());
    }

    [Fact]
    public async Task AddDuplicateTransfer()
    {
        Transfer mockTransfer = new(2, "TR00001", 0, 9229, "Uncompleted", []);

        bool IsAdded1 = await _service.AddTransfer(mockTransfer);

        Assert.True(IsAdded1);
        Assert.Equal([mockTransfer], await _service.GetTransfers());

        bool IsAdded2 = await _service.AddTransfer(mockTransfer);

        Assert.False(IsAdded2);
        Assert.Equal([mockTransfer], await _service.GetTransfers());

        bool IsRemoved = await _service.RemoveTransfer(2);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetTransfers());
    }

    [Fact]
    public async Task AddTransferWithDuplicateId()
    {
        Transfer mockTransfer1 = new(2, "TR00001", 0, 9229, "Uncompleted", []);
        Transfer mockTransfer2 = new(2, "TR00002", 9229, 9284, "Uncompleted", []);

        bool IsAdded1 = await _service.AddTransfer(mockTransfer1);

        Assert.True(IsAdded1);
        Assert.Equal([mockTransfer1], await _service.GetTransfers());

        bool IsAdded2 = await _service.AddTransfer(mockTransfer2);

        Assert.False(IsAdded2);
        Assert.Equal([mockTransfer1], await _service.GetTransfers());

        bool IsRemoved = await _service.RemoveTransfer(2);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetTransfers());
    }

    [Fact]
    public async Task UpdateTransfer()
    {
        Transfer mockTransfer1 = new(2, "TR00001", 0, 9229, "Uncompleted", []);
        Transfer mockTransfer2 = new(2, "TR00002", 9229, 9284, "Uncompleted", []);

        bool IsAdded = await _service.AddTransfer(mockTransfer1);

        Assert.True(IsAdded);
        Assert.Equal([mockTransfer1], await _service.GetTransfers());

        bool IsUpdated = await _service.UpdateTransfer(mockTransfer2);

        Assert.True(IsUpdated);
        Assert.Equal([mockTransfer2], await _service.GetTransfers());
        Assert.NotEqual([mockTransfer1], await _service.GetTransfers());

        await _service.RemoveTransfer(2);
    }

    [Fact]
    public async Task UpdateTransferWrong()
    {
        Transfer mockTransfer1 = new(2, "TR00001", 0, 9229, "Completed", []);
        Transfer mockTransfer2 = new(2, "TR00002", 9229, 9284, "Completed", []);

        bool IsAdded = await _service.AddTransfer(mockTransfer1);

        Assert.False(IsAdded);
        Assert.Equal([], await _service.GetTransfers());

        bool IsUpdated = await _service.UpdateTransfer(mockTransfer2);

        Assert.False(IsUpdated);
        Assert.Equal([], await _service.GetTransfers());
        Assert.NotEqual([mockTransfer1, mockTransfer2], await _service.GetTransfers());

        await _service.RemoveTransfer(2);
    }

    [Fact]
    public async Task RemoveTransfer()
    {
        Transfer mockTransfer1 = new(2, "TR00001", 0, 9229, "Uncompleted", []);

        bool IsAdded = await _service.AddTransfer(mockTransfer1);

        Assert.True(IsAdded);
        Assert.Equal([mockTransfer1], await _service.GetTransfers());

        bool IsRemoved = await _service.RemoveTransfer(2);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetTransfers());
    }
}