using Xunit;
using Microsoft.EntityFrameworkCore;

public class ItemTypeTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ItemTypeAccess _itemTypeAccess;
    private readonly ItemTypeServices _service;

    public ItemTypeTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _itemTypeAccess = new ItemTypeAccess(_dbContext);
        _service = new(_itemTypeAccess);
    }

    [Fact]
    public async Task GetAllItemTypes()
    {
        ItemType mockItemType = new(1, "Desktop", "Fast and big");

        Assert.Empty(await _service.GetItemTypes());

        await _service.AddItemType(mockItemType);

        Assert.Equal([mockItemType], await _service.GetItemTypes());

        await _service.RemoveItemType(1);

        Assert.Empty(await _service.GetItemTypes());
    }

    [Fact]
    public async Task GetItemType()
    {
        ItemType mockItemType = new(1, "Desktop", "Fast and big");

        bool IsAdded = await _service.AddItemType(mockItemType);

        Assert.True(IsAdded);
        Assert.Equal(mockItemType, await _service.GetItemType(1));
        Assert.Null(await _service.GetItemType(0));

        bool IsRemoved = await _service.RemoveItemType(1);

        Assert.True(IsRemoved);
        Assert.Null(await _service.GetItemType(1));
    }

    [Fact]
    public async Task AddItemTypeGood()
    {
        ItemType mockItemType = new(1, "Desktop", "Fast and big");

        Assert.Empty(await _service.GetItemTypes());

        bool IsAdded = await _service.AddItemType(mockItemType);

        Assert.True(IsAdded);
        Assert.Equal([mockItemType], await _service.GetItemTypes());

        bool IsRemoved = await _service.RemoveItemType(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemTypes());
    }
}