using Xunit;
using Microsoft.EntityFrameworkCore;

public class ClientTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ClientAccess _clientAccess;
    private readonly ClientServices _service;

    public ClientTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;

        _dbContext = new(options);

        // Create a new instance of Access with the in-memory DbContext
        _clientAccess = new(_dbContext);

        // Create new instance of Service
        _service = new(_clientAccess);
    }

    [Fact]
    public async Task GetAllClients()
    {
        Client mockClient = new(1, "testName", "LOC1", "testCity", "1234AB", "testProvince", "testCountry", "testName", "testPhone", "testEmail");

        Assert.Empty(await _service.GetClients());

        await _service.AddClient(mockClient);

        Assert.Equal(await _service.GetClient(mockClient.Id), mockClient);

        await _service.RemoveClient(1);
        Assert.Empty(await _service.GetClients());
    }

    [Fact]
    public async Task GetClient()
    {
        Client mockClient = new(1, "testName", "LOC1", "testCity", "1234AB", "testProvince", "testCountry", "testName", "testPhone", "testEmail");

        await _service.AddClient(mockClient);

        Assert.Equal(await _service.GetClient(mockClient.Id), mockClient);

        Assert.Null(await _service.GetClient(0));
        await _service.RemoveClient(1);

        Assert.Null(await _service.GetClient(1));
    }

    [Fact]
    public async Task AddGoodClient()
    {
        Client mockClient = new(1, "testName", "LOC1", "testCity", "1234AB", "testProvince", "testCountry", "testName", "testPhone", "testEmail");

        Assert.Empty(await _service.GetClients());

        bool IsAdded = await _service.AddClient(mockClient);

        Assert.True(IsAdded);
        Assert.Equal(await _service.GetClient(1), mockClient);

        await _service.RemoveClient(1);

        Assert.Empty(await _service.GetClients());
    }

    [Fact]
    public async Task AddDubplicateClient()
    {
        Client mockClient = new(1, "testName", "LOC1", "testCity", "1234AB", "testProvince", "testCountry", "testName", "testPhone", "testEmail");

        bool IsAdded1 = await _service.AddClient(mockClient);

        Assert.True(IsAdded1);
        Assert.Equal(mockClient, await _service.GetClient(1));

        bool IsAdded2 = await _service.AddClient(mockClient);

        Assert.False(IsAdded2);
        Assert.Equal(mockClient, await _service.GetClient(1));

        await _service.RemoveClient(1);

        Assert.Empty(await _service.GetClients());
    }

    [Fact]
    public async Task AddClientWithDuplicateId()
    {
        Client mockClient1 = new(1, "testName", "LOC1", "testCity", "1234AB", "testProvince", "testCountry", "testName", "testPhone", "testEmail");
        Client mockClient2 = new(1, "testName2", "LOC2", "testCity2", "1235AB", "testProvince2", "testCountry2", "testName2", "testPhone2", "testEmail2");

        bool IsAdded1 = await _service.AddClient(mockClient1);

        Assert.True(IsAdded1);
        Assert.Equal(await _service.GetClient(1), mockClient1);

        bool IsAdded2 = await _service.AddClient(mockClient2);

        Assert.False(IsAdded2);
        Assert.Equal(await _service.GetClients(), [mockClient1]);

        await _service.RemoveClient(1);

        Assert.Empty(await _service.GetClients());
    }

    [Fact]
    public async Task UpdateClient()
    {
        Client mockClient1 = new(1, "testName", "LOC1", "testCity", "1234AB", "testProvince", "testCountry", "testName", "testPhone", "testEmail");
        Client mockClient2 = new(1, "testName2", "LOC2", "testCity2", "1235AB", "testProvince2", "testCountry2", "testName2", "testPhone2", "testEmail2");

        bool IsAdded = await _service.AddClient(mockClient1);

        Assert.True(IsAdded);
        Assert.Equal(await _service.GetClient(1), mockClient1);

        bool IsUpdated = await _service.UpdateClient(mockClient2);

        Assert.True(IsUpdated);
        Assert.Equal(mockClient2, await _service.GetClient(1));
        Assert.NotEqual(mockClient1, await _service.GetClient(1));

        await _service.RemoveClient(1);
    }

    [Fact]
    public async Task RemoveClient()
    {
        Client mockClient = new(1, "testName", "LOC1", "testCity", "1234AB", "testProvince", "testCountry", "testName", "testPhone", "testEmail");

        bool IsAdded = await _service.AddClient(mockClient);

        Assert.True(IsAdded);

        List<Client> clients = await _service.GetClients();

        Assert.Equal(mockClient, clients[0]);

        bool IsRemoved = await _service.RemoveClient(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetClients());
    }
}