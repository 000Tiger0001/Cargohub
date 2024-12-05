using Xunit;
using Microsoft.EntityFrameworkCore;

public class TransferTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly TransferAccess _transferAccess;
    private readonly TransferServices _service;

    public TransferTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;

        _dbContext = new ApplicationDbContext(options);

        // Create a new instance of Access with the in-memory DbContext
        _transferAccess = new TransferAccess(_dbContext);

        // Create new instance of Service
        _service = new(_transferAccess);
    }

    [Fact]
    public async Task GetAllTransfers()
    {
        Transfer mockTransfer = new(1, "TR00001", 0, 9229, "Completed", [new(7435, 23)]);

        Assert.Empty(await _service.GetTransfers());

        await _service.AddTransfer(mockTransfer);

        Assert.Equal([mockTransfer], await _service.GetTransfers());

        await _service.RemoveTransfer(1);

        Assert.Empty(await _service.GetTransfers());
    }

    [Fact]
    public async Task GetTransfer()
    {
        Transfer mockTransfer = new(2, "TR00001", 0, 9229, "Completed", [new(7435, 23)]);

        await _service.AddTransfer(mockTransfer);

        Assert.Equal(mockTransfer, await _service.GetTransfer(1));
        Assert.Null(await _service.GetTransfer(0));

        await _service.RemoveTransfer(1);

        Assert.Null(await _service.GetTransfer(1));
    }

    [Fact]
    public async Task GetItemsInTransfer()
    {
        Transfer mockTransfer = new(2, "TR00001", 0, 9229, "Completed", [new(7435, 23)]);
        List<TransferItemMovement> items = [new(7435, 23)];

        await _service.AddTransfer(mockTransfer);

        List<TransferItemMovement>? transferItems = await _service.GetItemsInTransfer(2);
        int listSize = transferItems!.Count;

        Assert.Equal(items, transferItems);
        Assert.Equal(1, listSize);

        await _service.RemoveTransfer(2);

        Assert.Empty(await _service.GetTransfers());
    }

    [Fact]
    public async Task AddTransfer()
    {
        Transfer mockTransfer = new(2, "TR00001", 0, 9229, "Completed", [new(7435, 23)]);

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
        Transfer mockTransfer = new(2, "TR00001", 0, 9229, "Completed", [new(7435, 23)]);

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
        Transfer mockTransfer1 = new(2, "TR00001", 0, 9229, "Completed", [new(7435, 23)]);
        Transfer mockTransfer2 = new(2, "TR00002", 9229, 9284, "Completed", [new(7435, 23)]);

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
        Transfer mockTransfer1 = new(2, "TR00001", 0, 9229, "Completed", [new(7435, 23)]);
        Transfer mockTransfer2 = new(2, "TR00002", 9229, 9284, "Completed", [new(7435, 23)]);

        bool IsAdded = await _service.AddTransfer(mockTransfer1);

        Assert.True(IsAdded);
        Assert.Equal([mockTransfer1], await _service.GetTransfers());

        bool IsUpdated = await _service.UpdateTransfer(mockTransfer2);

        Assert.True(IsUpdated);
        Assert.Equal([mockTransfer2], await _service.GetTransfers());
        Assert.NotEqual([mockTransfer1], await _service.GetTransfers());

        await _service.RemoveTransfer(2);
    }
}