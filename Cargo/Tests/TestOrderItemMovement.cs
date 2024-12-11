using Xunit;
using Microsoft.EntityFrameworkCore;

public class OrderItemMovementTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly TransferItemMovementAccess _transferItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly OrderItemMovementServices _service;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _serviceItems;

    public OrderItemMovementTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _transferItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _orderItemMovementAccess = new OrderItemMovementAccess(_dbContext);
        _service = new(_orderItemMovementAccess);
        _itemAccess = new(_dbContext);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _transferItemMovementAccess, _shipmentItemMovementAccess);
    }
    [Fact]
    public async Task GetOrderItemMovements()
    {
        Item item = new() { Id = 1 };
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 2) { Id = 1 };
        OrderItemMovement sIM2 = new(1, 3) { Id = 2 };

        await _service.AddOrderItemMovement(sIM1);
        await _service.AddOrderItemMovement(sIM2);

        Assert.Equal([sIM1, sIM2], await _service.GetOrderItemMovements());

        await _service.RemoveOrderItemMovement(1);
        await _service.RemoveOrderItemMovement(2);
    }

    [Fact]
    public async Task GetOrderItemMovement()
    {
        Item item = new() { Id = 1 };
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 2) { Id = 1 };

        await _service.AddOrderItemMovement(sIM1);

        Assert.Equal(sIM1, await _service.GetOrderItemMovement(1));
        Assert.Null(await _service.GetOrderItemMovement(-1));
        Assert.Null(await _service.GetOrderItemMovement(2));
        await _service.RemoveOrderItemMovement(1);
    }

    [Fact]
    public async Task AddOrderItemMovement()
    {
        Item item = new() { Id = 1 };
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 3) { Id = 1 };
        bool success = await _service.AddOrderItemMovement(sIM1);

        Assert.True(success);
        Assert.Equal([sIM1], await _service.GetOrderItemMovements());

        await _service.RemoveOrderItemMovement(1);
    }

    [Fact]
    public async Task AddDuplicateId()
    {
        Item item = new() { Id = 1 };
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 3) { Id = 1 };

        await _service.AddOrderItemMovement(sIM1);

        Assert.Equal([sIM1], await _service.GetOrderItemMovements());

        OrderItemMovement sIM2 = new(1, 2) { Id = 1 };

        bool success = await _service.AddOrderItemMovement(sIM2);

        Assert.False(success);
        Assert.Equal([sIM1], await _service.GetOrderItemMovements());

        await _service.RemoveOrderItemMovement(1);
    }

    [Fact]
    public async Task RemoveOrderItemMovement()
    {
        Item item = new() { Id = 1 };
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 3) { Id = 1 };
        OrderItemMovement sIM2 = new(1, 2) { Id = 1 };

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
        Item item = new() { Id = 1 };
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetOrderItemMovements(), []);

        OrderItemMovement sIM1 = new(1, 2) { Id = 1 };

        await _service.AddOrderItemMovement(sIM1);

        Assert.Equal([sIM1], await _service.GetOrderItemMovements());

        OrderItemMovement sIM2 = new(1, 3) { Id = 1 };

        bool success = await _service.UpdateOrderItemMovement(sIM2);

        Assert.True(success);
        Assert.Equal([sIM2], await _service.GetOrderItemMovements());

        await _service.RemoveOrderItemMovement(2);
    }

}