using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class LocationControllers : Controller
{
    private readonly LocationServices _locationAccess;

    public LocationControllers(LocationServices locationServices)
    {
        _locationAccess = locationServices;
    }

    [HttpGet("locations")]
    public async Task<IActionResult> GetAllLocations()
    {
        List<Location> locations = await _locationAccess.GetLocations();
        return Ok(locations);
    }

    [HttpGet("location/{locationId}")]
    public async Task<IActionResult> GetLocation(int locationId)
    {
        Location? location = await _locationAccess.GetLocation(locationId);
        if (location is not null) return Ok(location);
        return BadRequest("There is no location with the given id. ");
    }

    [HttpGet("warehouse/{warehouseId}/locations")]
    public async Task<IActionResult> GetLocationsFromWarehouse(int warehouseId)
    {
        List<Location> locations = await _locationAccess.GetLocationsInWarehouse(warehouseId);
        if (locations.Count > 0) return Ok(locations);
        return BadRequest("There are no locations with the given warehouse id. ");
    }

    [HttpPost("location")]
    public async Task<IActionResult> AddLocation([FromBody] Location location)
    {
        if (location.WarehouseId == 0 || location.Id == 0 || location.Code == default || location.Name == default) BadRequest("Data incomplete. ");

        bool IsAdded = await _locationAccess.AddLocation(location);
        if (!IsAdded) return BadRequest("This location already exists. ");
        return Ok("Location added. ");
    }

    [HttpPut("location")]
    public async Task<IActionResult> UpdateLocation([FromBody] Location location)
    {
        if (location.Id == 0) return BadRequest("Location doesn't have an id. ");

        bool IsUpdated = await _locationAccess.UpdateLocation(location);
        if (!IsUpdated) return BadRequest("Location couldn't be updated. ");
        return Ok("Location updated. ");
    }

    [HttpDelete("location/{locationId}")]
    public async Task<IActionResult> RemoveLocation(int locationId)
    {
        if (locationId == 0) return BadRequest("Cannot remove location with empty id. ");

        bool IsRemoved = await _locationAccess.RemoveLocation(locationId);
        if (!IsRemoved) return BadRequest("Can't remove location");
        return Ok("Location removed. ");
    }
}
