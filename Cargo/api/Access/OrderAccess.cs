public class OrderAccess : BaseAccess<Order>
{
    private readonly ItemMovementAccess<Order, OrderItemMovement> _itemMovementAccess;

    public OrderAccess(ApplicationDbContext context) : base(context)
    {
        _itemMovementAccess = new ItemMovementAccess<Order, OrderItemMovement>(context);
    }

    public override async Task<List<Order>> GetAll()
    {
        return await _itemMovementAccess.GetAll();
    }

    public override async Task<Order?> GetById(int id)
    {
        return await _itemMovementAccess.GetById(id);
    }

    public override async Task<bool> Update(Order order)
    {
        return await _itemMovementAccess.Update(order);
    }
}