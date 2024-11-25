using Microsoft.AspNetCore.Http.HttpResults;

public class OrderServices
{
    private OrderAccess _orderAccess;
    private bool _debug;
    private List<Order> _testOrders;

    public OrderServices(OrderAccess orderAccess, bool debug)
    {
        _orderAccess = orderAccess;
        _debug = debug;
        _testOrders = [];
    }
    public async Task<List<Order>> GetOrders() => _debug ? _testOrders : await _orderAccess.GetAll();

    public async Task<Order?> GetOrder(int orderId) => _debug ? _testOrders.FirstOrDefault(o => o.Id == orderId) : await _orderAccess.GetById(orderId)!;

    public async Task<List<OrderItemMovement>> GetItemsInOrder(int orderId)
    {
        Order? order = await GetOrder(orderId)!;
        return order!.Items!;
    }

    public async Task<List<Order>> GetOrdersInShipment(int shipmentId)
    {
        List<Order> orders = await GetOrders();
        return orders.Where(o => o.ShipmentId == shipmentId).ToList();
    }

    public async Task<List<Order>> GetOrdersForClient(int clientId)
    {
        List<Order> orders = await GetOrders();
        return orders.Where(o => o.ShipTo == clientId || o.BillTo == clientId).ToList();
    }

    public async Task<bool> AddOrder(Order order)
    {
        if (order is null || order.SourceId == 0 || order.OrderDate == default || order.RequestDate == default || order.Reference == "" || order.ExtraReference == "" || (order.BillTo == 0 && order.ShipTo == 0) || order.Items == null || order.Notes == "" || order.OrderDate == default || order.OrderStatus == "" || order.PickingNotes == "" || order.ShipmentId == 0 || order.ShippingNotes == "" || order.TotalAmount == 0.0 || order.Totaldiscount == 0.0 || order.TotalSurcharge == 0.0 || order.TotalTax == 0.0 || order.WarehouseId == 0) return false;
        List<Order> orders = await GetOrders();
        Order doubleOrder = orders.FirstOrDefault(o => o.SourceId == order.SourceId && o.BillTo == order.BillTo && o.ExtraReference == order.ExtraReference && o.Items == order.Items && o.Notes == order.Notes && o.OrderDate == order.OrderDate && o.OrderStatus == order.OrderStatus && o.PickingNotes == order.PickingNotes && o.Reference == order.Reference && o.RequestDate == order.RequestDate && o.ShipmentId == order.ShipmentId && o.ShippingNotes == o.ShippingNotes && o.ShipTo == order.ShipTo && o.TotalAmount == order.TotalAmount && o.Totaldiscount == order.Totaldiscount)!;
        if (doubleOrder is null) return false;
        if (!_debug) return await _orderAccess.Add(order);
        _testOrders.Add(order);
        return true;
    }

    public async Task<bool> UpdateOrder(Order order)
    {
        if (order is null || order.Id == 0) return false;
        order.UpdatedAt = DateTime.Now;
        if (!_debug) return await _orderAccess.Update(order);
        int foundOrderIndex = _testOrders.FindIndex(o => o.Id == order.Id);
        if (foundOrderIndex == -1) return false;
        _testOrders[foundOrderIndex] = order;
        return true;
    }

    public async Task<bool> RemoveOrder(int orderId) => _debug ? _testOrders.Remove(_testOrders.FirstOrDefault(o => o.Id == orderId)!) : await _orderAccess.Remove(orderId);
}