public class OrderServices
{
    private OrderAccess _orderAccess;
    private OrderItemMovementAccess _orderItemMovementAccess;
    private InventoryAccess _inventoryAccess;
    private readonly ItemAccess _itemAccess;
    private readonly UserAccess _userAccess;

    public OrderServices(OrderAccess orderAccess, OrderItemMovementAccess orderItemMovementAccess, InventoryAccess inventoryAccess, ItemAccess itemAccess, UserAccess userAccess)
    {
        _orderItemMovementAccess = orderItemMovementAccess;
        _orderAccess = orderAccess;
        _inventoryAccess = inventoryAccess;
        _itemAccess = itemAccess;
        _userAccess = userAccess;
    }
    public async Task<List<Order>> GetOrders() => await _orderAccess.GetAll();

    public async Task<Order?> GetOrder(int orderId) => await _orderAccess.GetById(orderId);

    public async Task<List<Order?>> GetOrdersForUser(int userId)
    {
        User? user = await _userAccess.GetById(userId);
        List<Order> orders = await GetOrders();
        return orders.Where(order => order.WarehouseId == user?.WarehouseId).ToList()!;
    }

    public async Task<List<Order>> GetOrdersInShipmentForUser(int shipmentId, int userId)
    {
        List<Order?> orders = await GetOrdersForUser(userId);
        return orders.Where(o => o!.ShipmentId == shipmentId).ToList()!;
    }

    public async Task<List<Order>> GetOrdersForClientForUser(int clientId, int userId)
    {
        List<Order?> orders = await GetOrdersForUser(userId);
        return orders.Where(o => o!.SourceId == clientId).ToList()!;
    }

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
        if (order is null || order.SourceId == 0 || order.OrderDate == default || order.RequestDate == default || order.Reference == "" || order.ExtraReference == "" || order.Items == null || order.Notes == "" || order.OrderDate == default || order.OrderStatus == "" || order.PickingNotes == "" || order.ShipmentId == 0 || order.ShippingNotes == "" || order.TotalAmount == 0.0 || order.TotalDiscount == 0.0 || order.TotalSurcharge == 0.0 || order.TotalTax == 0.0 || order.WarehouseId == 0 || order.OrderStatus == "Delivered") return false;
        if (order.OrderStatus != "Delivered" && order.OrderStatus != "Packed" && order.OrderStatus != "Shipped" && order.OrderStatus != "Pending") return false;
        List<Order> orders = await GetOrders();
        Order doubleOrder = orders.FirstOrDefault(o => o.SourceId == order.SourceId && o.BillTo == order.BillTo && o.ExtraReference == order.ExtraReference && o.Items == order.Items && o.Notes == order.Notes && o.OrderDate == order.OrderDate && o.OrderStatus == order.OrderStatus && o.PickingNotes == order.PickingNotes && o.Reference == order.Reference && o.RequestDate == order.RequestDate && o.ShipmentId == order.ShipmentId && o.ShippingNotes == o.ShippingNotes && o.ShipTo == order.ShipTo && o.TotalAmount == order.TotalAmount && o.TotalDiscount == order.TotalDiscount)!;
        if (doubleOrder is not null) return false;
        foreach (OrderItemMovement orderItemMovement in order.Items) if (await _itemAccess.GetById(orderItemMovement.ItemId) is null) return false;
        return await _orderAccess.Add(order);
    }

    public async Task<bool> UpdateOrder(Order order)
    {
        if (order is null || order.Id <= 0) return false;
        if (order.OrderStatus != "Delivered" && order.OrderStatus != "Packed" && order.OrderStatus != "Shipped" && order.OrderStatus != "Pending") return false; order.UpdatedAt = DateTime.Now;
        return await _orderAccess.Update(order);
    }

    private async Task<bool> _updateItemsinInventory(Order order, Inventory inventory, int oldAmount, OrderItemMovement newItemMovement)
    {
        if (order!.OrderStatus == "Pending")
        {
            int changeamount = newItemMovement.Amount - oldAmount;
            inventory.TotalOrdered += changeamount;
            inventory.TotalOnHand -= changeamount;
            await _inventoryAccess.Update(inventory);
            return true;
        }
        else if (order!.OrderStatus == "Packed")
        {
            int changeamount = newItemMovement.Amount - oldAmount;
            inventory.TotalAllocated += changeamount;
            inventory.TotalOnHand -= changeamount;
            await _inventoryAccess.Update(inventory);
            return true;
        }
        return false;
    }
    public async Task<bool> UpdateItemsinOrders(int orderId, List<OrderItemMovement> items)
    {
        try
        {
            //check for old items to update or to remove
            Order? order = await _orderAccess.GetById(orderId);
            List<OrderItemMovement> orderItemMovements = await _orderItemMovementAccess.GetAllByOrderId(orderId);
            foreach (OrderItemMovement orderItemMovement in orderItemMovements)
            {
                OrderItemMovement changeInItem = items.First(item => item.Id == orderItemMovement.ItemId);
                Inventory? inventory = await _inventoryAccess.GetInventoryByItemId(orderItemMovement.ItemId);
                changeInItem.Id = orderItemMovement.Id;

                //update existing item
                if (items.Select(item => item.ItemId).Contains(orderItemMovement.ItemId))
                {
                    //item is getting updated
                    await _orderItemMovementAccess.Update(changeInItem);


                }
                //remove item completely
                else
                {
                    await _orderItemMovementAccess.Remove(orderItemMovement.Id);
                }

                await _updateItemsinInventory(order!, inventory!, orderItemMovement.Amount, changeInItem);
            }

            //check for new Items that were not in old
            foreach (OrderItemMovement orderItemMovementNew in items)
            {
                Inventory? inventory = await _inventoryAccess.GetInventoryByItemId(orderItemMovementNew.ItemId);
                if (!orderItemMovements.Contains(orderItemMovementNew))
                {
                    await _orderItemMovementAccess.Add(orderItemMovementNew);

                    //update inventory based on order
                    await _updateItemsinInventory(order!, inventory!, 0, orderItemMovementNew);
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveOrder(int orderId) => await _orderAccess.Remove(orderId);
}