public class OrderItemMovementServices
{
    private readonly OrderItemMovementAccess _access;

    public OrderItemMovementServices(OrderItemMovementAccess orderItemMovementAccess)
    {
        _access = orderItemMovementAccess;
    }

    public async Task<List<OrderItemMovement>> GetOrderItemMovements() => await _access.GetAll();

    public async Task<OrderItemMovement?> GetOrderItemMovement(int OrderItemMovementId) => await _access.GetById(OrderItemMovementId);

    public async Task<bool> AddOrderItemMovement(OrderItemMovement orderItemMovement)
    {
        List<OrderItemMovement> orderItemMovements = await GetOrderItemMovements();
        if (orderItemMovements.FirstOrDefault(o => o.Id == orderItemMovement.Id || (o.ItemId == orderItemMovement.ItemId && o.Amount == orderItemMovement.Amount)) is not null) return false;
        return await _access.Add(orderItemMovement);
    }

    public async Task<bool> UpdateOrderItemMovement(OrderItemMovement orderItemMovement)
    {
        if (orderItemMovement is null || orderItemMovement.Id <= 0 || orderItemMovement.ItemId <= 0 || orderItemMovement.Amount <= 0) return false;
        return await _access.Update(orderItemMovement);
    }

    public async Task<bool> RemoveOrderItemMovement(int orderItemMovementId) => await _access.Remove(orderItemMovementId);
}