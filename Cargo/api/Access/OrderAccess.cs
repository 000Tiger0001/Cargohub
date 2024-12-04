using Microsoft.EntityFrameworkCore;

public class OrderAccess : BaseAccess<Order>
{
    public OrderAccess(ApplicationDbContext context) : base(context) { }

    public override async Task<List<Order>> GetAll()
    {
        List<Order> orders = await _context.Set<Order>().AsNoTracking()
            .Include(order => order.Items) // Include OrderItemMovement entities
            .ToListAsync();
        return orders;
    }

    public override async Task<Order?> GetById(int orderId)
    {
        Order? order = await _context.Set<Order>().AsNoTracking()
            .Include(o => o.Items)  // Explicitly include related OrderItemMovements
            .FirstOrDefaultAsync(o => o.Id == orderId);

        return order;
    }

    public override async Task<bool> Update(Order order)
    {
        return true;
    }
}