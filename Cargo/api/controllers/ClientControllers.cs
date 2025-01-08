using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ClientControllers : Controller
{
    private ClientServices _clientService;

    public ClientControllers(ClientServices clientServices)
    {
        _clientService = clientServices;
    }

    [HttpGet("clients")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Analyst", "Logistics", "Sales"])]
    public async Task<IActionResult> GetClients() => Ok(await _clientService.GetClients());

    [HttpGet("client/{clientId}")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Analyst", "Logistics", "Sales"])]
    public async Task<IActionResult> GetClient(int clientId)
    {
        if (clientId <= 0) return BadRequest("Can't get client with this id. ");

        Client? client = await _clientService.GetClient(clientId);
        if (client is null) return BadRequest("Client not found. ");
        return Ok(client);
    }

    [HttpPost("client")]
    [RightsFilter(["Admin", "Warehouse Manager", "Logistics", "Sales"])]
    public async Task<IActionResult> AddClient([FromBody] Client client)
    {
        if (client is null || client.Name == "" || client.Address == "" || client.City == "" || client.ZipCode == "" || client.Province == "" || client.Country == "" || client.ContactName == "" || client.ContactPhone == "" || client.ContactEmail == "") return BadRequest("Not enough info given. ");

        bool IsAdded = await _clientService.AddClient(client);
        if (!IsAdded) return BadRequest("Couldn't add client. ");
        return Ok("Client added. ");
    }

    [HttpPut("client")]
    [RightsFilter(["Admin", "Warehouse Manager", "Logistics", "Sales"])]
    public async Task<IActionResult> UpdateClient([FromBody] Client client)
    {
        if (client is null || client.Id <= 0) return BadRequest("Not enough info given. ");

        bool IsUpdated = await _clientService.UpdateClient(client);
        if (!IsUpdated) return BadRequest("Couldn't update client. ");
        return Ok("Client updated. ");
    }

    [HttpDelete("client/{clientId}")]
    [RightsFilter(["Admin"])]
    public async Task<IActionResult> RemoveClient(int clientId)
    {
        if (clientId <= 0) return BadRequest("Can't remove client with this id. ");

        bool IsRemoved = await _clientService.RemoveClient(clientId);
        if (!IsRemoved) return BadRequest("Can't remove client. ");
        return Ok("Client removed. ");
    }
}