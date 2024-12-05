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

        // Fetch the existing entity
        var existingEntity = await GetById(entity.Id);
        if (existingEntity == null) return false;

        // Handle item movements (update or add)
        if (entity.Items != null)
        {
            // remove any items that are present in the existing list but not in the updated list
            var updatedItemsIds = entity.Items.Select(i => i.Id).ToList();
            var itemsToRemove = existingEntity.Items?.Where(i => !updatedItemsIds.Contains(i.Id)).ToList();
            if (itemsToRemove is not null) _context.Set<TItemMovement>().RemoveRange(itemsToRemove);

            // Items to update or add
            foreach (var item in entity.Items)
            {
                var existingItem = await _context.Set<TItemMovement>().FindAsync(item.Id);
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

        // Update the entity itself (if any other properties have changed)
        _context.Entry(entity).State = EntityState.Modified;
        var changes = await _context.SaveChangesAsync();
        ClearChangeTracker();
        return changes > 0;
    }
}
