public class OrderServices
{
    public async Task<List<Order>> GetOrders() => await AccessJson.ReadJson<Order>();

    public async Task<Order> GetOrder(Guid orderId)
    {
        List<Order> orders = await GetOrders();
        return orders.FirstOrDefault(o => o.Id == orderId)!;
    }
}