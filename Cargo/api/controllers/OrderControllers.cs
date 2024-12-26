using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class OrderControllers : Controller
{
    private OrderServices _orderServices;

    public OrderControllers(OrderServices orderServices)
    {
        _orderServices = orderServices;
    }

    [HttpGet("orders")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Operative", "Supervisor", "Analyst", "Sales", "Logistics"])]
    public async Task<IActionResult> GetOrders() => Ok(await _orderServices.GetOrders());

    [HttpGet("order/{orderId}")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Operative", "Supervisor", "Analyst", "Sales", "Logistics"])]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        if (orderId <= 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetOrder(orderId));
    }

    [HttpGet("order/{orderId}/items")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Operative", "Supervisor", "Analyst", "Sales", "Logistics"])]
    public async Task<IActionResult> GetItemsInOrder(int orderId)
    {
        if (orderId <= 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetItemsInOrder(orderId));
    }

    [HttpGet("shipment/{shipmentId}/orders")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Operative", "Supervisor", "Analyst", "Sales", "Logistics"])]
    public async Task<IActionResult> GetOrdersInShipment(int shipmentId)
    {
        if (shipmentId <= 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetOrdersInShipment(shipmentId));
    }

    [HttpGet("client/{clientId}/orders")]
    [RightsFilter(["Admin", "Warehouse Manager", "Inventory Manager", "Floor Manager", "Operative", "Supervisor", "Analyst", "Sales", "Logistics"])]
    public async Task<IActionResult> GetOrdersForClient(int clientId)
    {
        if (clientId <= 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetOrdersForClient(clientId));
    }

    [HttpPost("order")]
    [RightsFilter(["Admin", "Warehouse Manager", "Sales", "Logistics"])]
    public async Task<IActionResult> AddOrder([FromBody] Order order)
    {
        if (order is null) return BadRequest("Not enough info. ");

        bool IsAdded = await _orderServices.AddOrder(order);
        if (!IsAdded) return BadRequest("Couldn't add order. ");
        return Ok("Order added. ");
    }

    [HttpPut("order")]
    [RightsFilter(["Admin", "Warehouse Manager", "Sales", "Logistics"])]
    public async Task<IActionResult> UpdateOrder([FromBody] Order order)
    {
        if (order.Id <= 0) return BadRequest("Can't update this order. ");

        bool IsUpdated = await _orderServices.UpdateOrder(order);
        if (!IsUpdated) return BadRequest("Couldn't update order. ");
        return Ok("Order updated. ");
    }

    [HttpPut("update-items-in-order")]
    [RightsFilter(["Admin", "Warehouse Manager", "Sales", "Logistics"])]
    public async Task<IActionResult> UpdateItemsInOrder([FromBody] List<OrderItemMovement> orderItemMovements, int orderId)
    {
        if (await _orderServices.UpdateItemsinOrders(orderId, orderItemMovements))
        {
            return Ok("Items updated");
        }
        return BadRequest("Couldn't update items.");
    }

    [HttpDelete("order/{orderId}")]
    [RightsFilter(["Admin", "Warehouse Manager"])]
    public async Task<IActionResult> DeleteOrder(int orderId)

    {
        if (orderId <= 0) return BadRequest("Can't remove order with this id. ");

        bool IsRemoved = await _orderServices.RemoveOrder(orderId);
        if (!IsRemoved) return BadRequest("Couldn't remove order. ");
        return Ok("Order removed. ");
    }
}