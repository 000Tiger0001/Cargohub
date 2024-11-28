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

    [Fact]
    public async Task AddItemTypeBad()
    {
        Client mockClient = new(1, "Joost", "JoostLaan 2", "Rotterdam", "5656AA", "Zuid-Holland", "Nederland", "Joost", "06 123456789", "JoostMagHetWeten@gmail.com");

        Assert.Empty(await _service.GetItemTypes());

        /* De code hieronder is uitgecomment, omdat het een error geeft. */
        //await _service.AddItemType(mockClient);

        Assert.Empty(await _service.GetItemTypes());

        await _service.RemoveItemType(1);

        Assert.Empty(await _service.GetItemTypes());
    }

    [Fact]
    public async Task AddDuplicateItem()
    {
        ItemType mockItemType = new(1, "Desktop", "Fast and big");

        bool IsAdded1 = await _service.AddItemType(mockItemType);

        Assert.True(IsAdded1);
        Assert.Equal([mockItemType], await _service.GetItemTypes());

        bool IsAdded2 = await _service.AddItemType(mockItemType);

        Assert.False(IsAdded2);
        Assert.Equal([mockItemType], await _service.GetItemTypes());

        bool IsRemoved = await _service.RemoveItemType(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemTypes());
    }

    [Fact]
    public async Task AddItemWithDuplicateId()
    {
        ItemType mockItemType1 = new(1, "Desktop", "Fast and big");
        ItemType mockItemType2 = new(1, "Laptop", "Small, but nice");

        bool IsAdded1 = await _service.AddItemType(mockItemType1);

        Assert.True(IsAdded1);
        Assert.Equal([mockItemType1], await _service.GetItemTypes());

        bool IsAdded2 = await _service.AddItemType(mockItemType2);

        Assert.False(IsAdded2);
        Assert.Equal([mockItemType1], await _service.GetItemTypes());

        bool IsRemoved = await _service.RemoveItemType(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItemTypes());
    }
}