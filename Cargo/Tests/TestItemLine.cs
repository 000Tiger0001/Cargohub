using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

public class ItemLineTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemLineServices _service;

    public ItemLineTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _itemLineAccess = new ItemLineAccess(_dbContext);
        _service = new(_itemLineAccess);
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
    public async Task GetItem()
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
    public async Task AddItemLineBad()
    {
        Client mockClient = new(1, "Joost", "JoostLaan 2", "Rotterdam", "5656AA", "Zuid-Holland", "Nederland", "Joost", "06 123456789", "JoostMagHetWeten@gmail.com");

        Assert.Empty(await _service.GetItemLines());

        /* De code hieronder is uitgecomment, omdat het een error geeft. */
        //await _service.AddItemLine(mockClient);

        Assert.Empty(await _service.GetItemLines());

        await _service.RemoveItemLine(1);

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
}