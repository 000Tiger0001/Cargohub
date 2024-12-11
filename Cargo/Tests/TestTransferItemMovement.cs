using Xunit;
using Microsoft.EntityFrameworkCore;

public class TransferItemMovementTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly TransferItemMovementAccess _TransferItemMovementAccess;
    private readonly TransferItemMovementServices _service;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _serviceItems;

    public TransferItemMovementTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _TransferItemMovementAccess = new TransferItemMovementAccess(_dbContext);
        _service = new(_TransferItemMovementAccess);
        _itemAccess = new(_dbContext);
        _serviceItems = new(_itemAccess);
    }
    [Fact]
    public async Task GetTransferItemMovements()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 2) { Id = 1 };
        TransferItemMovement sIM2 = new(1, 3) { Id = 2 };

        await _service.AddTransferItemMovement(sIM1);
        await _service.AddTransferItemMovement(sIM2);

        Assert.Equal([sIM1, sIM2], await _service.GetTransferItemMovements());

        await _service.RemoveTransferItemMovement(1);
        await _service.RemoveTransferItemMovement(2);
    }

    [Fact]
    public async Task GetTransferItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 2) { Id = 1 };

        await _service.AddTransferItemMovement(sIM1);

        Assert.Equal(sIM1, await _service.GetTransferItemMovement(1));
        Assert.Null(await _service.GetTransferItemMovement(-1));
        Assert.Null(await _service.GetTransferItemMovement(2));
        await _service.RemoveTransferItemMovement(1);
    }

    [Fact]
    public async Task AddTransferItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 3) { Id = 1 };
        bool success = await _service.AddTransferItemMovement(sIM1);

        Assert.True(success);
        Assert.Equal([sIM1], await _service.GetTransferItemMovements());

        await _service.RemoveTransferItemMovement(1);
    }

    [Fact]
    public async Task AddDuplicateId()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 3) { Id = 1 };

        await _service.AddTransferItemMovement(sIM1);

        Assert.Equal([sIM1], await _service.GetTransferItemMovements());

        TransferItemMovement sIM2 = new(1, 2) { Id = 1 };

        bool success = await _service.AddTransferItemMovement(sIM2);

        Assert.False(success);
        Assert.Equal([sIM1], await _service.GetTransferItemMovements());

        await _service.RemoveTransferItemMovement(1);
    }

    [Fact]
    public async Task RemoveTransferItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 3) { Id = 1 };
        TransferItemMovement sIM2 = new(1, 2) { Id = 1 };

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
    }


    [Fact]
    public async Task UpdateTransferItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetTransferItemMovements(), []);

        TransferItemMovement sIM1 = new(1, 2) { Id = 1 };

        await _service.AddTransferItemMovement(sIM1);

        Assert.Equal([sIM1], await _service.GetTransferItemMovements());

        TransferItemMovement sIM2 = new(1, 3) { Id = 1 };

        bool success = await _service.UpdateTransferItemMovement(sIM2);

        Assert.True(success);
        Assert.Equal([sIM2], await _service.GetTransferItemMovements());

        await _service.RemoveTransferItemMovement(2);
    }

}