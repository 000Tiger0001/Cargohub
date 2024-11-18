public class OrderServices
{
    public async Task<List<Order>> GetOrders() => await AccessJson.ReadJson<Order>();

    public async Task<Order> GetOrder(Guid orderId)
    {
        List<Order> orders = await GetOrders();
        return orders.FirstOrDefault(o => o.Id == orderId)!;
    }

    public async Task<Dictionary<Guid, int>> GetItemsInOrder(Guid orderId)
    {
        Order order = await GetOrder(orderId);
        return order.Items;
    }

    public async Task<List<Order>> GetOrdersInShipment(Guid shipmentId)
    {
        List<Order> orders = await GetOrders();
        return orders.Where(o => o.ShipmentId == shipmentId).ToList();
    }

    public async Task<List<Order>> getOrdersForClient(Guid clientId)
    {
        List<Order> orders = await GetOrders();
        return orders.Where(o => o.ShipTo == clientId || o.BillTo == clientId).ToList();
    }
}