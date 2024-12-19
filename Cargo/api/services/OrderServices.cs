using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.VisualBasic;
using SQLitePCL;

public class OrderServices
{
    private OrderAccess _orderAccess;
    private OrderItemMovementAccess _orderItemMovementAccess;
    private InventoryAccess _inventoryAccess;
    private ShipmentServices _shipmentServices;

    public OrderServices(OrderAccess orderAccess, OrderItemMovementAccess orderItemMovementAccess, InventoryAccess inventoryAccess, ShipmentServices shipmentServices)
    {
        _orderItemMovementAccess = orderItemMovementAccess;
        _orderAccess = orderAccess;
        _inventoryAccess = inventoryAccess;
        _shipmentServices = shipmentServices;
    }
    public async Task<List<Order>> GetOrders() => await _orderAccess.GetAll();

    public async Task<Order> GetOrder(int orderId) => await _orderAccess.GetById(orderId);

    public async Task<List<OrderItemMovement>> GetItemsInOrder(int orderId)
    {
        Order order = await GetOrder(orderId);
        return order.Items!;
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
        bool IsAdded = await _orderAccess.Add(order);
        return IsAdded;
    }

    public async Task<bool> UpdateOrder(Order order)
    {
        if (order is null || order.Id == 0) return false;

        order.UpdatedAt = DateTime.Now;
        return await _orderAccess.Update(order); ;
    }
    /*public async Task<List<ShipmentItemMovement> convertOrderItemMovement(List<OrderItemMovement> orderItemMovements){
        List<ShipmentItemMovement> shipmentItemMovements = new();
        foreach(OrderItemMovement orderItemMovement in orderItemMovements){
            ShipmentItemMovement shipmentItemMovement = new();

        }
    }*/

    private async Task<bool> _updateItemsinInventory(Order order, Inventory inventory, int oldAmount, OrderItemMovement newItemMovement)
    {
        if (order!.OrderStatus == "pending")
        {
            int changeamount = newItemMovement.Amount - oldAmount;
            inventory.TotalOrdered += changeamount;
            inventory.TotalOnHand -= changeamount;
            await _inventoryAccess.Update(inventory);
            return true;
        }
        else if (order!.OrderStatus == "packed")
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
            Order order = await _orderAccess.GetById(orderId);
            List<OrderItemMovement> orderItemMovements = await _orderItemMovementAccess.GetAllByOrderId(orderId);
            foreach (OrderItemMovement orderItemMovement in orderItemMovements)
            {
                OrderItemMovement changeInItem = items.First(item => item.Id == orderItemMovement.ItemId);
                Inventory inventory = await _inventoryAccess.GetInventoryByItemId(orderItemMovement.ItemId);
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

                await _updateItemsinInventory(order!, inventory, orderItemMovement.Amount, changeInItem);
            }

            //check for new Items that were not in old
            foreach (OrderItemMovement orderItemMovementNew in items)
            {
                Inventory inventory = await _inventoryAccess.GetInventoryByItemId(orderItemMovementNew.ItemId);
                if (!orderItemMovements.Contains(orderItemMovementNew))
                {
                    await _orderItemMovementAccess.Add(orderItemMovementNew);

                    //update inventory based on order
                    await _updateItemsinInventory(order!, inventory, 0, orderItemMovementNew);
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