using Xunit;
using Microsoft.EntityFrameworkCore;

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
}