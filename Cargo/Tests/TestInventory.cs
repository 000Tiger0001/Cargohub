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
        Inventory mockInventory = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        Assert.Equal(await _service.GetInventories(), []);

        await _service.AddInventory(mockInventory);

        Assert.Equal(await _service.GetInventories(), [mockInventory]);

        await _service.RemoveInventory(1);

        Assert.Equal(await _service.GetInventories(), []);
    }

    [Fact]
    public async Task GetInventory()
    {
        Inventory mockInventory = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        await _service.AddInventory(mockInventory);

        Assert.Equal(await _service.GetInventory(1), mockInventory);
        Assert.Null(await _service.GetInventory(0));

        await _service.RemoveInventory(1);

        Assert.Null(await _service.GetInventory(1));
    }

    [Fact]
    public async Task GetInventoriesForItems()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(2, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653, 3068, 3334, 20477, 20524, 17579, 2271, 2293, 22717], 194, 0, 139, 41, 55);
        
        await _service.AddInventory(mockInventory1);
        await _service.AddInventory(mockInventory2);

        Assert.Equal(await _service.GetInventories(), [mockInventory1, mockInventory2]);
        Assert.Equal(await _service.GetInventoriesforItem(1), [mockInventory1]);
        Assert.Equal(await _service.GetInventoriesforItem(2), [mockInventory2]);
        Assert.Equal(await _service.GetInventoriesforItem(3), []);

        bool IsRemoved1 = await _service.RemoveInventory(1);
        bool IsRemoved2 = await _service.RemoveInventory(2);

        Assert.True(IsRemoved1);
        Assert.True(IsRemoved2);
        Assert.Equal(await _service.GetInventories(), []);
    }
}