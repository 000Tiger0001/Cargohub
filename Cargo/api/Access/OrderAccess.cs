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
        if (order == null) return false;

        DetachEntity(order);

        var existingOrder = await GetById(order.Id!);
        if (existingOrder == null)
        {
            return false;
        }

        if (existingOrder.Items != null)
        {
            foreach (OrderItemMovement item in existingOrder.Items)
            {
                var existingItem = await _context.Set<OrderItemMovement>().FirstOrDefaultAsync(i => i.Id == item.Id);
                if (existingItem != null)
                {
                    existingItem.Amount = item.Amount;
                    existingItem.ItemId = item.ItemId;
                }
                else
                {
                    _context.Set<OrderItemMovement>().Add(item);
                }
            }
        }
        _context.Update(order);
        var changes = await _context.SaveChangesAsync();
        ClearChangeTracker();
        return changes > 0;
    }
}