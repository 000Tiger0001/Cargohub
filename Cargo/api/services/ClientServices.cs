public class ClientServices
{
    private readonly ClientAccess _clientAccess;

    public ClientServices(ClientAccess clientAccess)
    {
        _clientAccess = clientAccess;
    }

    public async Task<List<Client>> GetClients() => await _clientAccess.GetAll();

    public async Task<Client?> GetClient(int clientId) => await _clientAccess.GetById(clientId);

    public async Task<bool> AddClient(Client client)
    {
        if (client is null || client.Name == "" || client.Address == "" || client.City == "" || client.ZipCode == "" || client.Province == "" || client.Country == "" || client.ContactName == "" || client.ContactPhone == "" || client.ContactEmail == "") return false;
        List<Client> clients = await GetClients();
        Client doubleClient = clients.FirstOrDefault(c => c.Id == client.Id || (c.Name == client.Name && c.Address == client.Address && c.City == client.City && c.ZipCode == client.ZipCode && c.Province == client.Province && c.Country == client.Country && c.ContactName == client.ContactName && c.ContactPhone == client.ContactPhone && c.ContactEmail == client.ContactEmail))!;
        if (doubleClient is not null) return false;
        return await _clientAccess.Add(client);
    }

    public async Task<bool> UpdateClient(Client client)
    {
        if (client == null || client.Id <= 0) return false;
        client.UpdatedAt = DateTime.Now;
        return await _clientAccess.Update(client);
    }

    public async Task<bool> RemoveClient(int clientId) => await _clientAccess.Remove(clientId);
}