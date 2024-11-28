using Xunit;
using Microsoft.EntityFrameworkCore;

public class ItemTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _service;

    public ItemTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _itemAccess = new ItemAccess(_dbContext);
        _service = new(_itemAccess);
    }

    public async Task GetAllItems()
    {
        Item mockItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");

        Assert.Empty(await _service.GetItems());

        await _service.AddItem(mockItem);

        Assert.Equal([mockItem], await _service.GetItems());

        await _service.RemoveItem(1);

        Assert.Empty(await _service.GetItems());
    }

    public async Task GetItem()
    {
        Item mockItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");

        await _service.AddItem(mockItem);

        Assert.Equal(mockItem, await _service.GetItem(1));
        Assert.Null(await _service.GetItem(0));

        await _service.RemoveItem(1);

        Assert.Null(await _service.GetItem(1));
    }
}