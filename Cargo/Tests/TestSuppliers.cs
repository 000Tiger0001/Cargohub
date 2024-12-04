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

        // Create a new instance of ClientAccess with the in-memory DbContext
        _supplierAccess = new SupplierAccess(_dbContext);

        // Create new instance of clientService
        _service = new(_supplierAccess);
    }

    [Fact]
    public async Task GetAllSuppliers()
    {
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        Assert.Empty(await _service.GetSuppliers());

        await _service.AddSupplier(mockSupplier);

        Assert.Equal([mockSupplier], await _service.GetSuppliers());

        await _service.RemoveSupplier(1);

        Assert.Empty(await _service.GetSuppliers());
    }

    [Fact]
    public async Task GetSupplier()
    {
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _service.AddSupplier(mockSupplier);

        Assert.Equal(mockSupplier, await _service.GetSupplier(1));
        Assert.Null(await _service.GetSupplier(0));

        await _service.RemoveSupplier(1);

        Assert.Null(await _service.GetSupplier(1));
    }
}