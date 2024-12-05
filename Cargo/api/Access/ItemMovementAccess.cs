using Microsoft.EntityFrameworkCore;

public class ItemMovementAccess<TEntity, TItemMovement> : BaseAccess<TEntity>
    where TEntity : class, IHasId, IHasItemMovements<TItemMovement>
    where TItemMovement : class, IHasId
{
    public ItemMovementAccess(ApplicationDbContext context) : base(context) { }

    public override async Task<List<TEntity>> GetAll()
    {
        return await _context.Set<TEntity>()
            .AsNoTracking()
            .Include(entity => entity.Items)
            .ToListAsync();
    }

    public override async Task<TEntity?> GetById(int id)
    {
        return await _context.Set<TEntity>()
            .AsNoTracking()
            .Include(entity => entity.Items)
            .FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public override async Task<bool> Update(TEntity entity)
    {
        if (entity == null) return false;

        DetachEntity(entity);

        var existingEntity = await GetById(entity.Id);
        if (existingEntity == null) return false;

        // Handle item movements
        if (entity.Items != null)
        {
            foreach (var item in entity.Items)
            {
                var existingItem = await _context.Set<TItemMovement>().FirstOrDefaultAsync(i => i.Id == item.Id);
                if (existingItem != null)
                {
                    // Update existing item
                    _context.Entry(existingItem).CurrentValues.SetValues(item);
                }
                else
                {
                    // Add new item
                    _context.Set<TItemMovement>().Add(item);
                }
            }
        }

        // Update the entity
        _context.Update(entity);
        var changes = await _context.SaveChangesAsync();
        ClearChangeTracker();
        return changes > 0;
    }
}
