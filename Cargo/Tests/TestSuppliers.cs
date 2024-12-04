using Xunit;
using Microsoft.EntityFrameworkCore;

public class SupplierTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly SupplierAccess _supplierAccess;
    private readonly SupplierServices _service;

    public SupplierTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;

        _dbContext = new ApplicationDbContext(options);

        // Create a new instance of Access with the in-memory DbContext
        _supplierAccess = new SupplierAccess(_dbContext);

        // Create new instance of Service
        _service = new(_supplierAccess);
    }
}