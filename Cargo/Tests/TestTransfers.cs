using Xunit;
using Microsoft.EntityFrameworkCore;

public class TransferTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly TransferAccess _transferAccess;
    private readonly TransferServices _service;

    public TransferTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;

        _dbContext = new ApplicationDbContext(options);

        // Create a new instance of Access with the in-memory DbContext
        _transferAccess = new TransferAccess(_dbContext);

        // Create new instance of Service
        _service = new(_transferAccess);
    }

    [Fact]
    public async Task GetAllTransfers()
    {
        Transfer mockTransfer = new(1, "TR00001", 0, 9229, "Completed", [new(7435, 23)]);
        
        Assert.Empty(await _service.GetTransfers());
        
        await _service.AddTransfer(mockTransfer);

        Assert.Equal([mockTransfer], await _service.GetTransfers());

        await _service.RemoveTransfer(1);

        Assert.Empty(await _service.GetTransfers());
    }

    [Fact]
    public async Task GetTransfer()
    {
        Transfer mockTransfer = new(1, "TR00001", 0, 9229, "Completed", [new(7435, 23)]);

        await _service.AddTransfer(mockTransfer);

        Assert.Equal(mockTransfer, await _service.GetTransfer(1));
        Assert.Null(await _service.GetTransfer(0));

        await _service.RemoveTransfer(1);

        Assert.Null(await _service.GetTransfer(1));
    }
}