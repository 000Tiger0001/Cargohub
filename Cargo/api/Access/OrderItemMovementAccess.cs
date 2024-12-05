using Microsoft.EntityFrameworkCore;

public class OrderItemMovementAccess : BaseAccess<OrderItemMovement>
{
    public OrderItemMovementAccess(ApplicationDbContext context) : base(context) { }

    public async Task<List<OrderItemMovement?>> GetAllByOrderId(int orderId)
    {
        return DB.AsNoTracking().Where(entity => entity.OrderId == orderId)!.ToList()!;
    }

}