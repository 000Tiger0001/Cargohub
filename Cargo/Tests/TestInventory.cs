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
}