using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LocationControllerTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly LocationAccess _locationAccess;
    private readonly LocationControllers _controller;

    public LocationControllerTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase("cargohub") // In-memory database
                        .Options;

        _dbContext = new ApplicationDbContext(options);

        // Create a new instance of LocationAccess with the in-memory DbContext
        _locationAccess = new LocationAccess(_dbContext);

        // Initialize the controller with LocationAccess
        _controller = new LocationControllers(_locationAccess);
    }

    [Fact]
    public async Task GetAllLocations_ReturnsOkResult_WithLocations()
    {
        // Arrange
        var mockLocations = new List<Location>
        {
            new Location { Id = 1, WarehouseId = 1, Code = "LOC1", Name = "Location 1" },
            new Location { Id = 2, WarehouseId = 1, Code = "LOC2", Name = "Location 2" }
        };

        // Add the mock locations to the in-memory database
        await _dbContext.Locations.AddRangeAsync(mockLocations);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _controller.GetAllLocations();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var locations = Assert.IsType<List<Location>>(okResult.Value);
        Assert.Equal(2, locations.Count);
    }

    [Fact]
    public async Task GetLocation_ReturnsOkResult_WhenLocationExists()
    {
        // Arrange
        var locationId = 100000000;
        var mockLocation = new Location { Id = locationId, WarehouseId = 1, Code = "LOC1", Name = "Location 1" };

        // Add the mock location to the in-memory database
        await _dbContext.Locations.AddAsync(mockLocation);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _controller.GetLocation(locationId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var location = Assert.IsType<Location>(okResult.Value);
        Assert.Equal(locationId, location.Id);
    }

    [Fact]
    public async Task GetLocation_ReturnsBadRequest_WhenLocationDoesNotExist()
    {
        // Arrange
        var locationId = -1; // Non-existent location ID
        // Act
        var result = await _controller.GetLocation(locationId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("There is no location with the given id. ", badRequestResult.Value);
    }
}
