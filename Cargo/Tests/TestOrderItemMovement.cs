using Xunit;
using Microsoft.EntityFrameworkCore;

public class OrderItemMovementTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly OrderItemMovementAccess _OrderItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private readonly TransferItemMovementAccess _TransferItemMovementAccess;
    private readonly OrderItemMovementServices _service;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _serviceItems;

    public OrderItemMovementTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _OrderItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _TransferItemMovementAccess = new(_dbContext);
        _service = new(_OrderItemMovementAccess);
        _itemAccess = new(_dbContext);
        _serviceItems = new(_itemAccess, _OrderItemMovementAccess, _TransferItemMovementAccess, _shipmentItemMovementAccess);
    }
    [Fact]
    public async Task GetOrderItemMovements()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 1, 2);
        OrderItemMovement sIM2 = new(2, 1, 3);

        await _service.AddOrderItemMovement(sIM1);
        await _service.AddOrderItemMovement(sIM2);

        Assert.Equal([sIM1, sIM2], await _service.GetOrderItemMovements());

        await _service.RemoveOrderItemMovement(1);
        await _service.RemoveOrderItemMovement(2);
    }

    [Fact]
    public async Task GetOrderItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 1, 2);

        await _service.AddOrderItemMovement(sIM1);

        Assert.Equal(sIM1, await _service.GetOrderItemMovement(1));
        Assert.Null(await _service.GetOrderItemMovement(-1));
        Assert.Null(await _service.GetOrderItemMovement(2));
        await _service.RemoveOrderItemMovement(1);
    }

    [Fact]
    public async Task AddOrderItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 1, 3);
        bool success = await _service.AddOrderItemMovement(sIM1);

        Assert.True(success);
        Assert.Equal([sIM1], await _service.GetOrderItemMovements());

        await _service.RemoveOrderItemMovement(1);
    }

    [Fact]
    public async Task AddDuplicateId()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 1, 3);

        await _service.AddOrderItemMovement(sIM1);

        Assert.Equal([sIM1], await _service.GetOrderItemMovements());

        OrderItemMovement sIM2 = new(1, 1, 2);

        bool success = await _service.AddOrderItemMovement(sIM2);

        Assert.False(success);
        Assert.Equal([sIM1], await _service.GetOrderItemMovements());

        await _service.RemoveOrderItemMovement(1);
    }

    [Fact]
    public async Task RemoveOrderItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 1, 3);
        OrderItemMovement sIM2 = new(2, 1, 2);

        await _service.AddOrderItemMovement(sIM1);
        await _service.AddOrderItemMovement(sIM2);

        Assert.Equal([sIM1, sIM2], await _service.GetOrderItemMovements());

        bool success = await _service.RemoveOrderItemMovement(1);

        Assert.True(success);
        Assert.Equal([sIM2], await _service.GetOrderItemMovements());

        success = await _service.RemoveOrderItemMovement(3);

        Assert.False(success);
        Assert.Equal([sIM2], await _service.GetOrderItemMovements());

        await _service.RemoveOrderItemMovement(2);
    }


    [Fact]
    public async Task UpdateOrderItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 1, 2);

        await _service.AddOrderItemMovement(sIM1);

        Assert.Equal([sIM1], await _service.GetOrderItemMovements());

        OrderItemMovement sIM2 = new(1, 1, 3);

        bool success = await _service.UpdateOrderItemMovement(sIM2);

        Assert.True(success);
        Assert.Equal([sIM2], await _service.GetOrderItemMovements());

        await _service.RemoveOrderItemMovement(2);
    }

}