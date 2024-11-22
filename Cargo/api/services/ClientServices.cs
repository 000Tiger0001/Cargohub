public class ClientServices
{
    private ClientAccess _clientAccess;
    private bool _debug;
    public List<Client> testClients;

    public ClientServices(ClientAccess clientAccess, bool debug)
    {
        _clientAccess = clientAccess;
        _debug = debug;
        testClients = [];
    }

    public async Task<List<Client>> GetClients()
    {
        if (!_debug) return await _clientAccess.GetAll();
        return testClients;
    }

    public async Task<Client?> GetClient(int clientId)
    {
        if (!_debug) return await _clientAccess.GetById(clientId);
        return testClients.FirstOrDefault(c => c.Id == clientId);
    }

    public async Task<bool> AddClient(Client client)
    {
        if (client is null || client.Name == "" || client.Address == "" || client.City == "" || client.ZipCode == "" || client.Province == "" || client.Country == "" || client.ContactName == "" || client.ContactPhone == "" || client.ContactEmail == "") return false;
        List<Client> clients = await GetClients();
        Client doubleClient = clients.FirstOrDefault(c => c.Name == client.Name && c.Address == client.Address && c.City == client.City && c.ZipCode == client.ZipCode && c.Province == client.Province && c.Country == client.Country && c.ContactName == client.ContactName && c.ContactPhone == client.ContactPhone && c.ContactEmail == client.ContactEmail)!;
        if (doubleClient is not null) return false;
        if (!_debug) return await _clientAccess.Add(client);
        testClients.Add(client);
        return true;
    }

    public async Task<bool> UpdateClient(Client client)
    {
        if (client == null || client.Id == 0) return false;

        client.UpdatedAt = DateTime.Now;
        if (!_debug) return await _clientAccess.Update(client);
        int foundClientIndex = testClients.FindIndex(c => c.Id == client.Id);
        if (foundClientIndex == -1) return false;
        testClients[foundClientIndex] = client;
        return true;
    }

    public async Task<bool> RemoveClient(int clientId)
    {
        if (!_debug) await _clientAccess.Remove(clientId);
        Client foundClient = testClients.FirstOrDefault(c => c.Id == clientId)!;
        return testClients.Remove(foundClient);
    }
}