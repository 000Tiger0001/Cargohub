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
}