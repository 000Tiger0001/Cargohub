using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ClientControllers : Controller
{
    private ClientServices _clientService;

    public ClientControllers(ClientAccess clientAccess)
    {
        _clientService = new(clientAccess, false);
    }

    [HttpGet("clients")]
    public async Task<IActionResult> GetClients() => Ok(await _clientService.GetClients());

    [HttpGet("client")]
    public async Task<IActionResult> GetClient([FromQuery] int clientId)
    {
        if (clientId == 0) return BadRequest("Can't get client with empty id. ");

        Client? client = await _clientService.GetClient(clientId);
        if (client is null) return BadRequest("Client not found. ");
        return Ok(client);
    }

    [HttpPost("add-client")]
    public async Task<IActionResult> AddClient([FromBody] Client client)
    {
        if (client is null || client.Name == "" || client.Address == "" || client.City == "" || client.ZipCode == "" || client.Province == "" || client.Country == "" || client.ContactName == "" || client.ContactPhone == "" || client.ContactEmail == "") return BadRequest("Not enough info given. ");

        bool IsAdded = await _clientService.AddClient(client);
        if (!IsAdded) return BadRequest("Couldn't add client. ");
        return Ok("Client added. ");
    }

    [HttpPut("update-client")]
    public async Task<IActionResult> UpdateClient([FromBody] Client client)
    {
        if (client is null || client.Id == 0) return BadRequest("Not enough info given. ");

        bool IsUpdated = await _clientService.UpdateClient(client);
        if (!IsUpdated) return BadRequest("Couldn't update client. ");
        return Ok("Client updated. ");
    }

    public async Task<IActionResult> RemoveClient([FromQuery] int clientId)
    {
        if (clientId == 0) return BadRequest("Can't remove client with empty id. ");

        bool IsRemoved = await _clientService.RemoveClient(clientId);
        if (!IsRemoved) return BadRequest("Can't remove client. ");
        return Ok("Client removed. ");
    }
}