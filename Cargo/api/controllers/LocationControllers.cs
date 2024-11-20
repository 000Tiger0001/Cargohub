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


//     [HttpGet("/locations-from-warehouse")]
//     public async Task<IActionResult> GetLocationsFromWarehouse([FromQuery] Guid warehouseId)
//     {
//         List<Location> locations = await LS.GetLocationsInWarehouse(warehouseId);
//         if (locations.Count > 0) return Ok(locations);
//         return BadRequest("There are no locations with the given warehouse id. ");
//     }

//     [HttpPost("/add-location")]
//     public async Task<IActionResult> AddLocation([FromBody] Location location)
//     {
//         location.Id = Guid.NewGuid();
//         if (location.WarehouseId == Guid.Empty || location.Id == Guid.Empty || location.Code.Length == 0 || location.Name.Length == 0) BadRequest("Data incomplete. ");
    [HttpGet("locations-from-warehouse")]
    public async Task<IActionResult> GetLocationsFromWarehouse([FromQuery] Guid warehouseId)
    {
        List<Location> locations = await LS.GetLocationsInWarehouse(warehouseId);
        if (locations.Count > 0) return Ok(locations);
        return BadRequest("There are no locations with the given warehouse id. ");
    }

    [HttpPost("add-location")]
    public async Task<IActionResult> AddLocation([FromBody] Location location)
    {
        location.Id = Guid.NewGuid();
        if (location.WarehouseId == Guid.Empty || location.Id == Guid.Empty || location.Code.Length == 0 || location.Name.Length == 0) BadRequest("Data incomplete. ");

//         bool IsAdded = await LS.AddLocation(location);
//         if (!IsAdded) return BadRequest("This location already exists. ");
//         return Ok("Location added. ");
//     }


//     [HttpPut("/update-location")]
//     public async Task<IActionResult> UpdateLocation([FromBody] Location location)
//     {
//         if (location.Id == Guid.Empty) return BadRequest("Location doesn't have an id. ");

    [HttpPut("update-location")]
    public async Task<IActionResult> UpdateLocation([FromBody] Location location)
    {
        if (location.Id == Guid.Empty) return BadRequest("Location doesn't have an id. ");

//         bool IsUpdated = await LS.UpdateLocation(location);
//         if (!IsUpdated) return BadRequest("Location couldn't be updated. ");
//         return Ok("Location updated. ");
//     }

//     [HttpDelete("/remove-location")]
//     public async Task<IActionResult> RemoveLocation([FromQuery] Guid locationId)
//     {
//         if (locationId == Guid.Empty) return BadRequest("Cannot remove location with empty id. ");

    [HttpDelete("remove-location")]
    public async Task<IActionResult> RemoveLocation([FromQuery] Guid locationId)
    {
        if (locationId == Guid.Empty) return BadRequest("Cannot remove location with empty id. ");

//         bool IsRemoved = await LS.RemoveLocation(locationId);
//         if (!IsRemoved) return BadRequest("Couldn't removed location. ");
//         return Ok("Removed location. ");
//     }
// }