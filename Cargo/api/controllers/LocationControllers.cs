using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class LocationControllers : Controller
{
    LocationServices LS;
    private readonly LocationAccess _locationAccess;

    public LocationControllers(LocationServices ls, LocationAccess locationAccess)
    {
        LS = ls;
        _locationAccess = locationAccess;
    }

    [HttpGet("locations")]
    public async Task<IActionResult> GetAllLocations()
    {
        List<Location> locations = await _locationAccess.GetAll();
        return Ok(locations);
    }

    [HttpGet("location")]
    public async Task<IActionResult> GetLocation([FromQuery] int locationId)
    {
        Location? location = await _locationAccess.GetById(locationId);
        if (location is not null) return Ok(location);
        return BadRequest("There is no location with the given id. ");
    }
}
