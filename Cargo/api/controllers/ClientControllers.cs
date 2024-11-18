using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class ClientControllers : Controller
{
    ClientServices CS;

    public ClientControllers(ClientServices cs)
    {
        CS = cs;
    }

    [HttpGet("clients")]
    public async Task<IActionResult> GetClients()
    {
        List<Client> clients = await CS.GetClients();
        return Ok(clients);
    }

    [HttpGet("client")]
    public async Task<IActionResult> GetClient([FromQuery] Guid clientId)
    {
        if (clientId == Guid.Empty) return BadRequest("Can't get client with empty id. ");

        Client client = await CS.GetClient(clientId);
        if (client is null) return BadRequest("Client not found. ");
        return Ok(client);
    }

    [HttpPost("add-client")]
    public async Task<IActionResult> AddClient([FromBody] Client client)
    {
        if (client is null || client.Name == "" || client.Address == "" || client.City == "" || client.ZipCode == "" || client.Province == "" || client.Country == "" || client.ContactName == "" || client.ContactPhone == "" || client.ContactEmail == "") return BadRequest("Not enoug info given. ");

        bool IsAdded = await CS.AddClient(client);
        if (!IsAdded) return BadRequest("Couldn't add client. ");
        return Ok("Client added. ");
    }

    [HttpPut("update-client")]
    public async Task<IActionResult> UpdateClient([FromBody] Client client)
    {
        if (client is null || client.Id == Guid.Empty) return BadRequest("Not enough info given. ");

        bool IsUpdated = await CS.UpdateClient(client);
        if (!IsUpdated) return BadRequest("Couldn't update client. ");
        return Ok("Client updated. ");
    }

    public async Task<IActionResult> RemoveClient([FromQuery] Guid clientId)
    {
        if (clientId == Guid.Empty) return BadRequest("Can't remove client with empty id. ");

        bool IsRemoved = await CS.RemoveClient(clientId);
        if (!IsRemoved) return BadRequest("Can't remove client. ");
        return Ok("Client removed. ");
    }
}