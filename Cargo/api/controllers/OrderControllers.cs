using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class OrderControllers : Controller
{
    private OrderServices _orderServices;

    public OrderControllers(OrderAccess orderAccess)
    {
        _orderServices = new(orderAccess, false);
    }

    [HttpGet("get-orders")]
    public async Task<IActionResult> GetOrders() => Ok(await _orderServices.GetOrders());

    public async Task<IActionResult> GetOrder([FromQuery] int orderId)
    {
        if (orderId == 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetOrder(orderId));
    }

    [HttpGet("get-items-in-order")]
    public async Task<IActionResult> GetItemsInOrder([FromQuery] int orderId)
    {
        if (orderId == 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetItemsInOrder(orderId));
    }

    [HttpGet("get-orders-in-shipment")]
    public async Task<IActionResult> GetOrdersInShipment([FromQuery] int shipmentId)
    {
        if (shipmentId == 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetOrdersInShipment(shipmentId));
    }

    [HttpGet("get-orders-for-client")]
    public async Task<IActionResult> GetOrdersForClient([FromQuery] int clientId)
    {
        if (clientId == 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetOrdersForClient(clientId));
    }

    [HttpPost("add-order")]
    public async Task<IActionResult> AddOrder([FromBody] Order order)
    {
        if (order is null) return BadRequest("Not enough info. ");

        bool IsAdded = await _orderServices.AddOrder(order);
        if (!IsAdded) return BadRequest("Couldn't add order. ");
        return Ok("Order added. ");
    }

    [HttpPut("update-order")]
    public async Task<IActionResult> UpdateOrder([FromBody] Order order)
    {
        if (order.Id == 0) return BadRequest("Can't update this order. ");

        bool IsUpdated = await _orderServices.UpdateOrder(order);
        if (!IsUpdated) return BadRequest("Couldn't update order. ");
        return Ok("Order updated. ");
    }

    [HttpDelete("delete-order")]
    public async Task<IActionResult> DeleteOrder([FromQuery] int orderId)
    {
        if (orderId == 0) return BadRequest("Can't remove order with this id. ");

        bool IsRemoved = await _orderServices.RemoveOrder(orderId);
        if (!IsRemoved) return BadRequest("Couldn't remove order. ");
        return Ok("Order removed. ");
    }
}