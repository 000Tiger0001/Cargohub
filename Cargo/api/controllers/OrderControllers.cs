using Microsoft.AspNetCore.Mvc;

[Route("Cargohub")]
public class OrderControllers : Controller
{
    private OrderServices _orderServices;

    public OrderControllers(OrderServices orderServices)
    {
        _orderServices = orderServices;
    }

    [HttpGet("get-orders")]
    public async Task<IActionResult> GetOrders() => Ok(await _orderServices.GetOrders());

    public async Task<IActionResult> GetOrder([FromQuery] int orderId)
    {
        if (orderId == 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetOrder(orderId));
    }

    public async Task<IActionResult> GetItemsInOrder(int orderId)
    {
        if (orderId == 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetItemsInOrder(orderId));
    }

    public async Task<IActionResult> GetOrdersInShipment(int shipmentId)
    {
        if (shipmentId == 0) return BadRequest("Can't proccess this id. ");
        return Ok(await _orderServices.GetOrdersInShipment(shipmentId));
    }

    /*public async Task<IActionResult> GetOrdersForClient(int clientId)
    {
        if (clientId ==)
    }*/
}