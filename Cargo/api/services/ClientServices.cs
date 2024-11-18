public class ClientServices
{
    public async Task<List<Client>> GetClients() => await AccessJson.ReadJson<Client>();

    public async Task<Client> GetClient(Guid clientId)
    {
        List<Client> clients = await GetClients();
        return clients.FirstOrDefault(c => c.Id == clientId)!;
    }

    public async Task<bool> AddClient(Client client)
    {
        if (client is null || client.Name == "" || client.Address == "" || client.City == "" || client.ZipCode == "" || client.Province == "" || client.Country == "" || client.ContactName == "" || client.ContactPhone == "" || client.ContactEmail == "") return false;

        List<Client> clients = await GetClients();
        Client doubleClient = clients.FirstOrDefault(c => c.Name == client.Name && c.Address == client.Address && c.City == client.City && c.ZipCode == client.ZipCode && c.Province == client.Province && c.Country == client.Country && c.ContactName == client.ContactName && c.ContactPhone == client.ContactPhone && c.ContactEmail == client.ContactEmail)!;
        if (doubleClient is not null) return false;

        client.Id = Guid.NewGuid();
        await AccessJson.WriteJson(client);
        return true;
    }

    public async Task<bool> UpdateClient(Client client)
    {
        if (client.Id == Guid.Empty) return false;

        List<Client> clients = await GetClients();
        int foundClientIndex = clients.FindIndex(c => c.Id == client.Id);
        if (foundClientIndex == -1) return false;

        client.UpdatedAt = DateTime.Now;
        clients[foundClientIndex] = client;
        AccessJson.WriteJsonList(clients);
        return true;
    }

    public async Task<bool> RemoveClient(Guid clientId)
    {
        if (clientId == Guid.Empty) return false;

        List<Client> clients = await GetClients();
        Client foundClient = clients.FirstOrDefault(c => c.Id == clientId)!;
        if (foundClient is null) return false;

        clients.Remove(foundClient);
        AccessJson.WriteJsonList(clients);
        return true;
    }
}