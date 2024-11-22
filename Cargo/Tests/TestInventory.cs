using Xunit;
using Microsoft.EntityFrameworkCore;

public class InventoryControllerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly InventoryAccess _inventoryAccess;
    private readonly InventoryControllers _controller;
    private readonly InventoryServices _service;

    public InventoryControllerTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase("testhub") // In-memory database
                        .Options;

        _dbContext = new ApplicationDbContext(options);

        // Create a new instance of ClientAccess with the in-memory DbContext
        _inventoryAccess = new InventoryAccess(_dbContext);

        // Create new instance of clientService
        _service = new(_inventoryAccess, true);

        // Initialize the controller with ClientAccess
        _controller = new InventoryControllers(_service);
    }

    [Fact]
    public async Task GetAllInventories()
    {
        Inventory mockInventory = new(1, 1, "hoi", "", [1, 2, 3], 50, 0, 0, 0, 50);

        Assert.Equal(await _service.GetInventories(), []);

        await _service.AddInventory(mockInventory);

        Assert.Equal(await _service.GetInventories(), [mockInventory]);

        await _service.RemoveInventory(1);

        Assert.Equal(await _service.GetInventories(), []);
    }

    [Fact]
    public async Task GetInventory()
    {
        Inventory mockInventory = new(1, 1, "hoi", "", [1, 2, 3], 50, 0, 0, 0, 50);

        await _service.AddInventory(mockInventory);

        Assert.Equal(await _service.GetInventory(1), mockInventory);
        Assert.Null(await _service.GetInventory(0));

        await _service.RemoveInventory(1);

        Assert.Null(await _service.GetInventory(1));
    }
}