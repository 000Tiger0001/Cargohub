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
}