using Xunit;
using Microsoft.EntityFrameworkCore;

public class ItemLineTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ItemAccess _itemAccess;
    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemLineServices _service;

    public ItemLineTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new(options);
        _itemAccess = new(_dbContext);
        _itemLineAccess = new(_dbContext);
        _service = new(_itemLineAccess, _itemAccess);
    }

    [Fact]
    public async Task GetAllItemLines()
    {
        ItemLine mockItemLine = new(1, "Home Appliances", "Stuff for home");

        Assert.Empty(await _service.GetItemLines());

        await _service.AddItemLine(mockItemLine);

        Assert.Equal([mockItemLine], await _service.GetItemLines());

        await _service.RemoveItemLine(1);

        Assert.Empty(await _service.GetItemLines());
    }

    [Fact]
    public async Task GetItemLine()
    {
        ItemLine mockItemLine = new(1, "Home Appliances", "Stuff for home");

        bool IsAdded = await _service.AddItemLine(mockItemLine);

        Assert.True(IsAdded);
        Assert.Equal(mockItemLine, await _service.GetItemLine(1));
        Assert.Null(await _service.GetItemLine(0));

        bool IsRemoved = await _service.RemoveItemLine(1);

        Assert.True(IsRemoved);
        Assert.Null(await _service.GetItemLine(1));
    }

    [Fact]
    public async Task AddItemLineGood()
    {
        ItemLine mockItemLine = new(1, "Home Appliances", "Stuff for home");

        Assert.Empty(await _service.GetItemLines());

        bool IsAdded = await _service.AddItemLine(mockItemLine);

        Assert.True(IsAdded);
        Assert.Equal([mockItemLine], await _service.GetItemLines());

        bool IsRemoved = await _service.RemoveItemLine(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemLines());
    }

    [Fact]
    public async Task AddDuplicateItemLine()
    {
        ItemLine mockItemLine = new(1, "Home Appliances", "Stuff for home");

        bool IsAdded1 = await _service.AddItemLine(mockItemLine);

        Assert.True(IsAdded1);
        Assert.Equal([mockItemLine], await _service.GetItemLines());

        bool IsAdded2 = await _service.AddItemLine(mockItemLine);

        Assert.False(IsAdded2);
        Assert.Equal([mockItemLine], await _service.GetItemLines());

        bool IsRemoved = await _service.RemoveItemLine(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemLines());
    }

    [Fact]
    public async Task AddItemLineWithDuplicateId()
    {
        ItemLine mockItemLine1 = new(1, "Home Appliances", "Stuff for home");
        ItemLine mockItemLine2 = new(1, "Office Supplies", "Nice office");

        bool IsAdded1 = await _service.AddItemLine(mockItemLine1);

        Assert.True(IsAdded1);
        Assert.Equal([mockItemLine1], await _service.GetItemLines());

        bool IsAdded2 = await _service.AddItemLine(mockItemLine2);

        Assert.False(IsAdded2);
        Assert.Equal([mockItemLine1], await _service.GetItemLines());

        bool IsRemoved = await _service.RemoveItemLine(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemLines());
    }

    [Fact]
    public async Task UpdateItemLine()
    {
        ItemLine mockItemLine1 = new(1, "Home Appliances", "Stuff for home");
        ItemLine mockItemLine2 = new(1, "Office Supplies", "Nice office");

        bool IsAdded = await _service.AddItemLine(mockItemLine1);

        Assert.True(IsAdded);
        Assert.Equal([mockItemLine1], await _service.GetItemLines());

        bool IsUpdated = await _service.UpdateItemLine(mockItemLine2);

        Assert.True(IsUpdated);
        Assert.Equal([mockItemLine2], await _service.GetItemLines());
        Assert.NotEqual([mockItemLine1], await _service.GetItemLines());

        await _service.RemoveItemLine(1);
    }

    [Fact]
    public async Task RemoveItemLine()
    {
        ItemLine mockItemLine1 = new(1, "Home Appliances", "Stuff for home");

        bool IsAdded = await _service.AddItemLine(mockItemLine1);

        Assert.True(IsAdded);
        Assert.Equal([mockItemLine1], await _service.GetItemLines());

        bool IsRemoved = await _service.RemoveItemLine(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemLines());
    }
}