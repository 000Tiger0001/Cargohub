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
}