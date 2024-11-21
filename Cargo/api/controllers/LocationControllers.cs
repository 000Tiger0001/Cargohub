using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class LocationControllers : Controller
{
    private readonly LocationAccess _locationAccess;

    public LocationControllers(LocationAccess locationAccess)
    {
        _locationAccess = locationAccess;
    }

    [HttpGet("locations")]
    public async Task<IActionResult> GetAllLocations()
    {
        List<Location> locations = await LS.GetLocations();
        return Ok(locations);
    }

    [HttpGet("location")]
    public async Task<IActionResult> GetLocation([FromQuery] int locationId)
    {
        Location? location = await LS.GetLocation(locationId);
        if (location is not null) return Ok(location);
        return BadRequest("There is no location with the given id. ");
    }
<<<<<<< HEAD

    [HttpGet("locations-from-warehouse")]
    public async Task<IActionResult> GetLocationsFromWarehouse([FromQuery] int warehouseId)
    {
        List<Location> locations = await LS.GetLocationsInWarehouse(warehouseId);
        if (locations.Count > 0) return Ok(locations);
        return BadRequest("There are no locations with the given warehouse id. ");
    }

    [HttpPost("add-location")]
    public async Task<IActionResult> AddLocation([FromBody] Location location)
    {
        if (location.WarehouseId == 0 || location.Id == 0 || location.Code == default || location.Name == default) BadRequest("Data incomplete. ");

        bool IsAdded = await LS.AddLocation(location);
        if (!IsAdded) return BadRequest("This location already exists. ");
        return Ok("Location added. ");
    }

    [HttpPut("update-location")]
    public async Task<IActionResult> UpdateLocation([FromBody] Location location)
    {
        if (location.Id == 0) return BadRequest("Location doesn't have an id. ");

        bool IsUpdated = await LS.UpdateLocation(location);
        if (!IsUpdated) return BadRequest("Location couldn't be updated. ");
        return Ok("Location updated. ");
    }

    [HttpDelete("remove-location")]
    public async Task<IActionResult> RemoveLocation([FromQuery] int locationId)
    {
        if (locationId == 0) return BadRequest("Cannot remove location with empty id. ");

        bool IsRemoved = await LS.RemoveLocation(locationId);
        if (!IsRemoved) return BadRequest("Can't remove location");
        return Ok("Location removed. ");
    }
}
=======
}
>>>>>>> main
