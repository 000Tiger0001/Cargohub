using Xunit;
using Microsoft.EntityFrameworkCore;

public class ItemGroupTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ItemGroupAccess _itemGroupAccess;
    private readonly ItemGroupServices _service;
    private readonly ItemAccess _itemAccess;

    public ItemGroupTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new(options);
        _itemAccess = new(_dbContext);
        _itemGroupAccess = new(_dbContext);
        _service = new(_itemGroupAccess, _itemAccess);
    }

    [Fact]
    public async Task GetAllItemGroups()
    {
        ItemGroup mockItemGroup = new(1, "Hardware", "");

        Assert.Empty(await _service.GetItemGroups());

        await _service.AddItemGroup(mockItemGroup);

        Assert.Equal([mockItemGroup], await _service.GetItemGroups());

        await _service.RemoveItemGroup(1);

        Assert.Empty(await _service.GetItemGroups());
    }

    [Fact]
    public async Task GetItemGroup()
    {
        ItemGroup mockItemGroup = new(1, "Hardware", "");

        bool IsAdded = await _service.AddItemGroup(mockItemGroup);

        Assert.True(IsAdded);
        Assert.Equal(mockItemGroup, await _service.GetItemGroup(1));
        Assert.Null(await _service.GetItemGroup(0));

        bool IsRemoved = await _service.RemoveItemGroup(1);

        Assert.True(IsRemoved);
        Assert.Null(await _service.GetItemGroup(1));
    }

    [Fact]
    public async Task AddItemGroupGood()
    {
        ItemGroup mockItemGroup = new(1, "Hardware", "");

        Assert.Empty(await _service.GetItemGroups());

        bool IsAdded = await _service.AddItemGroup(mockItemGroup);

        Assert.True(IsAdded);
        Assert.Equal([mockItemGroup], await _service.GetItemGroups());

        bool IsRemoved = await _service.RemoveItemGroup(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemGroups());
    }

    [Fact]
    public async Task AddDuplicateItemGroup()
    {
        ItemGroup mockItemGroup = new(1, "Hardware", "");

        bool IsAdded = await _service.AddItemGroup(mockItemGroup);

        Assert.True(IsAdded);
        Assert.Equal([mockItemGroup], await _service.GetItemGroups());

        bool IsAdded1 = await _service.AddItemGroup(mockItemGroup);

        Assert.False(IsAdded1);
        Assert.Equal([mockItemGroup], await _service.GetItemGroups());

        bool IsRemoved = await _service.RemoveItemGroup(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemGroups());
    }

    [Fact]
    public async Task AddItemGroupWithDuplicateId()
    {
        ItemGroup mockItemGroup1 = new(1, "Hardware", "");
        ItemGroup mockItemGroup2 = new(1, "Software", "");

        bool IsAdded = await _service.AddItemGroup(mockItemGroup1);

        Assert.True(IsAdded);
        Assert.Equal([mockItemGroup1], await _service.GetItemGroups());

        bool IsAdded1 = await _service.AddItemGroup(mockItemGroup2);

        Assert.False(IsAdded1);
        Assert.Equal([mockItemGroup1], await _service.GetItemGroups());

        bool IsRemoved = await _service.RemoveItemGroup(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemGroups());
    }

    [Fact]
    public async Task UpdateItemGroupGood()
    {
        ItemGroup mockItemGroup1 = new(1, "Hardware", "");
        ItemGroup mockItemGroup2 = new(1, "Software", "");

        bool IsAdded = await _service.AddItemGroup(mockItemGroup1);

        Assert.True(IsAdded);
        Assert.Equal([mockItemGroup1], await _service.GetItemGroups());

        bool IsUpdated = await _service.UpdateItemGroup(mockItemGroup2);

        Assert.True(IsUpdated);
        Assert.Equal([mockItemGroup2], await _service.GetItemGroups());
        Assert.NotEqual([mockItemGroup1], await _service.GetItemGroups());

        await _service.RemoveItemGroup(1);
    }

    [Fact]
    public async Task RemoveItemGroup()
    {
        ItemGroup mockItemGroup1 = new(1, "Hardware", "");

        bool IsAdded = await _service.AddItemGroup(mockItemGroup1);

        Assert.True(IsAdded);
        Assert.Equal([mockItemGroup1], await _service.GetItemGroups());

        bool IsRemoved = await _service.RemoveItemGroup(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemGroups());
    }
}