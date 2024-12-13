using Xunit;
using Microsoft.EntityFrameworkCore;

public class InventoryTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly InventoryAccess _inventoryAccess;
    private readonly InventoryServices _service;

    public InventoryTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;

        _dbContext = new(options);

        // Create a new instance of Access with the in-memory DbContext
        _inventoryAccess = new(_dbContext);

        // Create new instance of Service
        _service = new(_inventoryAccess);
    }

    [Fact]
    public async Task GetAllInventories()
    {
        Inventory mockInventory = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        Assert.Empty(await _service.GetInventories());

        await _service.AddInventory(mockInventory);

        Assert.Equal([mockInventory], await _service.GetInventories());

        await _service.RemoveInventory(1);

        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task GetInventory()
    {
        Inventory mockInventory = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        await _service.AddInventory(mockInventory);

        Assert.Equal(mockInventory, await _service.GetInventory(1));
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

        Assert.Equal([mockInventory1, mockInventory2], await _service.GetInventories());
        Assert.Equal([mockInventory1], await _service.GetInventoriesforItem(1));
        Assert.Equal([mockInventory2], await _service.GetInventoriesforItem(2));
        Assert.Empty(await _service.GetInventoriesforItem(3));

        bool IsRemoved1 = await _service.RemoveInventory(1);
        bool IsRemoved2 = await _service.RemoveInventory(2);

        Assert.True(IsRemoved1);
        Assert.True(IsRemoved2);
        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task GetInventoryTotalsForItems()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(2, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653, 3068, 3334, 20477, 20524, 17579, 2271, 2293, 22717], 194, 0, 139, 0, 55);
        Inventory mockInventory3 = new(3, 3, "Cloned actuating artificial intelligence", "QVm03739H", [5321, 21960], 24, 0, 90, 68, -134);

        bool IsAdded1 = await _service.AddInventory(mockInventory1);
        bool IsAdded2 = await _service.AddInventory(mockInventory2);
        bool IsAdded3 = await _service.AddInventory(mockInventory3);

        Assert.True(IsAdded1);
        Assert.True(IsAdded2);
        Assert.True(IsAdded3);
        Assert.Equal([mockInventory1, mockInventory2, mockInventory3], await _service.GetInventories());

        Dictionary<string, int> totals1 = await _service.GetInventoryTotalsForItem(mockInventory1.Id);
        Assert.Equal(mockInventory1.TotalExpected, totals1["total_expected"]);
        Assert.Equal(mockInventory1.TotalOrdered, totals1["total_ordered"]);
        Assert.Equal(mockInventory1.TotalAllocated, totals1["total_allocated"]);
        Assert.Equal(mockInventory1.TotalAvailable, totals1["total_available"]);

        Dictionary<string, int> totals2 = await _service.GetInventoryTotalsForItem(mockInventory2.Id);
        Assert.Equal(mockInventory2.TotalExpected, totals2["total_expected"]);
        Assert.Equal(mockInventory2.TotalOrdered, totals2["total_ordered"]);
        Assert.Equal(mockInventory2.TotalAllocated, totals2["total_allocated"]);
        Assert.Equal(mockInventory2.TotalAvailable, totals2["total_available"]);

        Dictionary<string, int> totals3 = await _service.GetInventoryTotalsForItem(mockInventory3.Id);
        Assert.Equal(mockInventory3.TotalExpected, totals3["total_expected"]);
        Assert.Equal(mockInventory3.TotalOrdered, totals3["total_ordered"]);
        Assert.Equal(mockInventory3.TotalAllocated, totals3["total_allocated"]);
        Assert.Equal(mockInventory3.TotalAvailable, totals3["total_available"]);

        bool IsRemoved1 = await _service.RemoveInventory(1);
        bool IsRemoved2 = await _service.RemoveInventory(2);
        bool IsRemoved3 = await _service.RemoveInventory(3);

        Assert.True(IsRemoved1);
        Assert.True(IsRemoved2);
        Assert.True(IsRemoved3);
        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task AddInventoryGood()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        Assert.Empty(await _service.GetInventories());

        bool IsAdded = await _service.AddInventory(mockInventory1);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        await _service.RemoveInventory(1);

        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task AddDuplicateInventory()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        bool IsAdded = await _service.AddInventory(mockInventory1);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsAdded1 = await _service.AddInventory(mockInventory1);

        Assert.False(IsAdded1);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsRemoved = await _service.RemoveInventory(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task AddInventoryWithDuplicateId()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(1, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653, 3068, 3334, 20477, 20524, 17579, 2271, 2293, 22717], 194, 0, 139, 0, 55);

        bool IsAdded = await _service.AddInventory(mockInventory1);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsAdded1 = await _service.AddInventory(mockInventory2);

        Assert.False(IsAdded1);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsRemoved = await _service.RemoveInventory(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetInventories());
    }

    [Fact]
    public async Task UpdateInventoryGood()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);
        Inventory mockInventory2 = new(1, 2, "Focused transitional alliance", "nyg48736S", [19800, 23653, 3068, 3334, 20477, 20524, 17579, 2271, 2293, 22717], 194, 0, 139, 0, 55);

        bool IsAdded = await _service.AddInventory(mockInventory1);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsUpdated = await _service.UpdateInventory(mockInventory2);

        Assert.True(IsUpdated);
        Assert.Equal(mockInventory2, await _service.GetInventory(1));
        Assert.NotEqual(mockInventory1, await _service.GetInventory(1));

        await _service.RemoveInventory(1);
    }

    [Fact]
    public async Task RemoveInventory()
    {
        Inventory mockInventory1 = new(1, 1, "Face-to-face clear-thinking complexity", "sjQ23408K", [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], 262, 0, 80, 41, 141);

        bool IsAdded = await _service.AddInventory(mockInventory1);

        Assert.True(IsAdded);
        Assert.Equal([mockInventory1], await _service.GetInventories());

        bool IsRemoved = await _service.RemoveInventory(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetInventories());
    }
}