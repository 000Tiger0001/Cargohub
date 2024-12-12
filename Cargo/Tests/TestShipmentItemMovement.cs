using Xunit;
using Microsoft.EntityFrameworkCore;

public class ShipmentItemMovementTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private readonly TransferItemMovementAccess _TransferItemMovementAccess;
    private readonly ShipmentItemMovementServices _service;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _serviceItems;
    private readonly ItemGroupAccess _itemGroupAccess;
    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemTypeAccess _itemTypeAccess;

    public ShipmentItemMovementTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _orderItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _TransferItemMovementAccess = new(_dbContext);
        _service = new(_shipmentItemMovementAccess);
        _itemAccess = new(_dbContext);
        _itemGroupAccess = new(_dbContext);
        _itemLineAccess = new(_dbContext);
        _itemTypeAccess = new(_dbContext);
        _serviceItems = new(_itemAccess, _orderItemMovementAccess, _TransferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess);
    }
    [Fact]
    public async Task GetShipmentItemMovements()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetShipmentItemMovements(), []);

        ShipmentItemMovement sIM1 = new(1, 1, 2);
        ShipmentItemMovement sIM2 = new(2, 1, 3);

        await _service.AddShipmentItemMovement(sIM1);
        await _service.AddShipmentItemMovement(sIM2);

        Assert.Equal([sIM1, sIM2], await _service.GetShipmentItemMovements());

        await _service.RemoveShipmentItemMovement(1);
        await _service.RemoveShipmentItemMovement(2);
    }

    [Fact]
    public async Task GetShipmentItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetShipmentItemMovements(), []);

        ShipmentItemMovement sIM1 = new(1, 1, 2);

        await _service.AddShipmentItemMovement(sIM1);

        Assert.Equal(sIM1, await _service.GetShipmentItemMovement(1));
        Assert.Null(await _service.GetShipmentItemMovement(-1));
        Assert.Null(await _service.GetShipmentItemMovement(2));
        await _service.RemoveShipmentItemMovement(1);
    }

    [Fact]
    public async Task AddShipmentItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetShipmentItemMovements(), []);

        ShipmentItemMovement sIM1 = new(1, 1, 3);
        bool success = await _service.AddShipmentItemMovement(sIM1);

        Assert.True(success);
        Assert.Equal([sIM1], await _service.GetShipmentItemMovements());

        await _service.RemoveShipmentItemMovement(1);
    }

    [Fact]
    public async Task AddDuplicateId()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetShipmentItemMovements(), []);

        ShipmentItemMovement sIM1 = new(1, 1, 3);

        await _service.AddShipmentItemMovement(sIM1);

        Assert.Equal([sIM1], await _service.GetShipmentItemMovements());

        ShipmentItemMovement sIM2 = new(1, 1, 2);

        bool success = await _service.AddShipmentItemMovement(sIM2);

        Assert.False(success);
        Assert.Equal([sIM1], await _service.GetShipmentItemMovements());

        await _service.RemoveShipmentItemMovement(1);
    }

    [Fact]
    public async Task RemoveShipmentItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetShipmentItemMovements(), []);

        ShipmentItemMovement sIM1 = new(1, 1, 3);
        ShipmentItemMovement sIM2 = new(2, 1, 2);

        await _service.AddShipmentItemMovement(sIM1);
        await _service.AddShipmentItemMovement(sIM2);

        Assert.Equal([sIM1, sIM2], await _service.GetShipmentItemMovements());

        bool success = await _service.RemoveShipmentItemMovement(1);

        Assert.True(success);
        Assert.Equal([sIM2], await _service.GetShipmentItemMovements());

        success = await _service.RemoveShipmentItemMovement(3);

        Assert.False(success);
        Assert.Equal([sIM2], await _service.GetShipmentItemMovements());

        await _service.RemoveShipmentItemMovement(2);
    }


    [Fact]
    public async Task UpdateShipmentItemMovement()
    {
        Item item = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 0, 0, 0, 0, 0, 0, 0, null!, "E-86805-uTM");
        await _serviceItems.AddItem(item);
        Assert.Equal(await _serviceItems.GetItem(1), item);

        Assert.Equal(await _service.GetShipmentItemMovements(), []);

        ShipmentItemMovement sIM1 = new(1, 1, 2);

        await _service.AddShipmentItemMovement(sIM1);

        Assert.Equal([sIM1], await _service.GetShipmentItemMovements());

        ShipmentItemMovement sIM2 = new(1, 1, 3);

        bool success = await _service.UpdateShipmentItemMovement(sIM2);

        Assert.True(success);
        Assert.Equal([sIM2], await _service.GetShipmentItemMovements());

        await _service.RemoveShipmentItemMovement(2);
    }

}