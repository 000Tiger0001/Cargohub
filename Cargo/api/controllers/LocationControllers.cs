using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class LocationControllers : Controller
{
    LocationServices LS;

    public LocationControllers(LocationServices ls)
    {
        LS = ls;
    }

    [HttpGet("/locations")]
    public async Task<IActionResult> GetAllLocations()
    {
        List<Location> locations = await LS.GetLocations();
        return Ok(locations);
    }

    [HttpGet("/location")]
    public async Task<IActionResult> GetLocation([FromQuery] Guid locationId)
    {
        Location location = await LS.GetLocation(locationId);
        if (location is not null) return Ok(location);
        return BadRequest("There is no location with the given id. ");
    }

    [HttpGet("/locations-from-warehouse")]
    public async Task<IActionResult> GetLocationsFromWarehouse([FromQuery] Guid warehouseId)
    {
        List<Location> locations = await LS.GetLocationsInWarehouse(warehouseId);
        if (locations.Count > 0) return Ok(locations);
        return BadRequest("There are locations with the given warehouse id. ");
    }

    [HttpPost("/add-location")]
    public async Task<IActionResult> AddLocation([FromBody] Location location)
    {
        location.Id = Guid.NewGuid();
        if (location.WarehouseId == Guid.Empty || location.Id == Guid.Empty || location.Code.Length == 0 || location.Name.Length == 0) BadRequest("Data incomplete. ");

        bool IsAdded = await LS.AddLocation(location);
        if (!IsAdded) return BadRequest("This location already exists. ");
        return Ok("Location added. ");
    }

    [HttpPut("/update-location")]
    public async Task<IActionResult> UpdateLocation([FromBody] Location location)
    {
        if (location.Id == Guid.Empty) return BadRequest("Location doesn't have an id. ");

        bool IsUpdated = await LS.UpdateLocation(location);
        if (!IsUpdated) return BadRequest("Location couldn't be updated. ");
        return Ok("Location updated. ");
    }

    [HttpDelete("/remove-location")]
    public async Task<IActionResult> RemoveLocation([FromQuery] Guid locationId)
    {
        if (locationId == Guid.Empty) return BadRequest("Cannot remove location with empty id. ");

        bool IsRemoved = await LS.RemoveLocation(locationId);
        if (!IsRemoved) return BadRequest("Couldn't removed location. ");
        return Ok("Removed location. ");
    }
}