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

        // Create new instance of Service
        _service = new(_transferAccess);
        _itemAccess = new(_dbContext);
        _itemGroupAccess = new(_dbContext);
        _itemLineAccess = new(_dbContext);
        _itemTypeAccess = new(_dbContext);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _transferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess);
    }

    [Fact]
    public async Task GetAllTransfers()
    {
        Transfer mockTransfer = new(1, "TR00001", 0, 9229, "Completed", []);

        Assert.Empty(await _service.GetTransfers());

        await _service.AddTransfer(mockTransfer);

        Assert.Equal([mockTransfer], await _service.GetTransfers());

        await _service.RemoveTransfer(1);

        Assert.Empty(await _service.GetTransfers());
    }

    [Fact]
    public async Task GetTransfer()
    {
        Transfer mockTransfer = new(1, "TR00001", 0, 9229, "Completed", []);

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
        TransferItemMovement mockItem1 = new(7435, 23);

        Item item1 = new(7435, "hdaffhhds1", "random1", "r1", "5555 EE1", "hoie1", "jooh1", 0, 0, 0, 100, 100, 100, 0, "0000", "0000");

        bool IsItemAdded1 = await _serviceItems.AddItem(item1);

        Assert.True(IsItemAdded1);
        Assert.Equal([item1], await _serviceItems.GetItems());

        List<TransferItemMovement> items = [mockItem1];
        Transfer mockTransfer = new(2, "TR00001", 0, 9229, "Completed", items);

        await _service.AddTransfer(mockTransfer);

        List<TransferItemMovement>? transferItems = await _service.GetItemsInTransfer(2);
        int listSize = transferItems!.Count;

        Assert.Equal(items, transferItems);
        Assert.Equal(1, listSize);

        await _service.RemoveTransfer(2);

        Assert.Empty(await _service.GetTransfers());

        bool IsItemRemoved1 = await _serviceItems.RemoveItem(7435);

        Assert.True(IsItemRemoved1);
        Assert.Empty(await _serviceItems.GetItems());
    }

    [Fact]
    public async Task AddTransfer()
    {
        Transfer mockTransfer = new(1, "TR00001", 0, 9229, "Completed", []);

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
        Transfer mockTransfer = new(2, "TR00001", 0, 9229, "Completed", []);

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
        Transfer mockTransfer1 = new(2, "TR00001", 0, 9229, "Completed", []);
        Transfer mockTransfer2 = new(2, "TR00002", 9229, 9284, "Completed", []);

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
        Transfer mockTransfer1 = new(2, "TR00001", 0, 9229, "Completed", []);
        Transfer mockTransfer2 = new(2, "TR00002", 9229, 9284, "Completed", []);

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
    public async Task RemoveTransfer()
    {
        Transfer mockTransfer1 = new(2, "TR00001", 0, 9229, "Completed", []);

        bool IsAdded = await _service.AddTransfer(mockTransfer1);

        Assert.True(IsAdded);
        Assert.Equal([mockTransfer1], await _service.GetTransfers());

        bool IsRemoved = await _service.RemoveTransfer(2);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetTransfers());
    }
}