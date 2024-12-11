using Microsoft.AspNetCore.Http.HttpResults;

public class OrderServices
{
    private OrderAccess _orderAccess;

    public OrderServices(OrderAccess orderAccess)
    {
        _orderAccess = orderAccess;
    }
    public async Task<List<Order>> GetOrders() => await _orderAccess.GetAll();

    public async Task<Order?> GetOrder(int orderId) => await _orderAccess.GetById(orderId)!;

    public async Task<List<OrderItemMovement>> GetItemsInOrder(int orderId)
    {
        Order? order = await GetOrder(orderId);
        if (order is null || order.Items is null || order.Items.Count <= 0) return [];
        return order.Items;
    }

    public async Task<List<Order>> GetOrdersInShipment(int shipmentId)
    {
        List<Order> orders = await GetOrders();
        return orders.Where(o => o.ShipmentId == shipmentId).ToList();
    }

    public async Task<List<Order>> GetOrdersForClient(int clientId)
    {
        List<Order> orders = await GetOrders();
        return orders.Where(o => o.SourceId == clientId).ToList();
    }

    public async Task<bool> AddOrder(Order order)
    {
        if (order is null || order.SourceId == 0 || order.OrderDate == default || order.RequestDate == default || order.Reference == "" || order.ExtraReference == "" || order.Items == null || order.Notes == "" || order.OrderDate == default || order.OrderStatus == "" || order.PickingNotes == "" || order.ShipmentId == 0 || order.ShippingNotes == "" || order.TotalAmount == 0.0 || order.Totaldiscount == 0.0 || order.TotalSurcharge == 0.0 || order.TotalTax == 0.0 || order.WarehouseId == 0) return false;
        List<Order> orders = await GetOrders();
        Order doubleOrder = orders.FirstOrDefault(o => o.SourceId == order.SourceId && o.BillTo == order.BillTo && o.ExtraReference == order.ExtraReference && o.Items == order.Items && o.Notes == order.Notes && o.OrderDate == order.OrderDate && o.OrderStatus == order.OrderStatus && o.PickingNotes == order.PickingNotes && o.Reference == order.Reference && o.RequestDate == order.RequestDate && o.ShipmentId == order.ShipmentId && o.ShippingNotes == o.ShippingNotes && o.ShipTo == order.ShipTo && o.TotalAmount == order.TotalAmount && o.Totaldiscount == order.Totaldiscount)!;
        if (doubleOrder is not null) return false;
        return await _orderAccess.Add(order);
    }

    public async Task<bool> UpdateOrder(Order order)
    {
        if (order is null || order.Id == 0) return false;
        order.UpdatedAt = DateTime.Now;
        return await _orderAccess.Update(order);
    }

    public async Task<bool> RemoveOrder(int orderId) => await _orderAccess.Remove(orderId);
}