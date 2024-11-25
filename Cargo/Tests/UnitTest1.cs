// using Xunit;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Mvc;

// public class LocationControllerTests
// {
//     private readonly ApplicationDbContext _dbContext;
//     private readonly LocationAccess _locationAccess;
//     private readonly LocationControllers _controller;
//     private readonly LocationServices _service;

//     public LocationControllerTests()
//     {
//         // Use an in-memory SQLite database for testing
//         var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                         .UseInMemoryDatabase("cargohub") // In-memory database
//                         .Options;

//         _dbContext = new ApplicationDbContext(options);

//         // Create a new instance of LocationAccess with the in-memory DbContext
//         _locationAccess = new LocationAccess(_dbContext);

//         // Create new instance of locationService
//         _service = new(_locationAccess);

//         // Initialize the controller with LocationAccess
//         _controller = new LocationControllers(_service);
//     }

//     [Fact]
//     public async Task GetAllLocations_ReturnsOkResult_WithLocations()
//     {
//         // Arrange
//         List<Location> mockLocations =
//         [
//             new() { Id = 1, WarehouseId = 1, Code = "LOC1", Name = "Location 1" },
//             new() { Id = 2, WarehouseId = 1, Code = "LOC2", Name = "Location 2" }
//         ];

//         // Add the mock locations to the in-memory database
//         await _dbContext.Locations!.AddRangeAsync(mockLocations);
//         await _dbContext.SaveChangesAsync();

//         // Act
//         IActionResult result = await _controller.GetAllLocations();

//         // Assert
//         OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
//         Assert.Equal(200, okResult.StatusCode);

//         List<Location> locations = Assert.IsType<List<Location>>(okResult.Value);
//         Assert.Equal(2, locations.Count);
//     }

//     [Fact]
//     public async Task GetLocation_ReturnsOkResult_WhenLocationExists()
//     {
//         // Arrange
//         int locationId = 100000000;
//         Location mockLocation = new() { Id = locationId, WarehouseId = 1, Code = "LOC1", Name = "Location 1" };

//         // Add the mock location to the in-memory database
//         await _dbContext.Locations!.AddAsync(mockLocation);
//         await _dbContext.SaveChangesAsync();

//         // Act
//         IActionResult result = await _controller.GetLocation(locationId);

//         // Assert
//         OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
//         Assert.Equal(200, okResult.StatusCode);

//         Location location = Assert.IsType<Location>(okResult.Value);
//         Assert.Equal(locationId, location.Id);
//     }

//     [Fact]
//     public async Task GetLocation_ReturnsBadRequest_WhenLocationDoesNotExist()
//     {
//         // Arrange
//         int locationId = -1; // Non-existent location ID
//         // Act
//         IActionResult result = await _controller.GetLocation(locationId);

//         // Assert
//         BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//         Assert.Equal(400, badRequestResult.StatusCode);
//         Assert.Equal("There is no location with the given id. ", badRequestResult.Value);
//     }
// }
