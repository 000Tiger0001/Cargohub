using Microsoft.EntityFrameworkCore;

public abstract class BaseAccess<T> where T : class, IHasId
{
    protected readonly DbContext _context;
    protected DbSet<T> DB;

    public BaseAccess(DbContext context)
    {
        _context = context;
        DB = _context.Set<T>();
    }

    public virtual async Task<List<T>> GetAll()
    {
        return await DB.AsNoTracking().ToListAsync();
    }

    public virtual async Task<T?> GetById(int id)
    {
        return await DB.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id)!;
    }


    public async Task<bool> AddMany(List<T> data)
    {
        foreach (var entity in data)
        {
            if (entity == null) continue;

            // Detach the entity from the context if it is already being tracked
            DetachEntity(entity);
            var existingEntity = await GetById(entity.Id);

            if (existingEntity != null) continue;

            await DB.AddAsync(entity);
        }

        var changes = await _context.SaveChangesAsync();
        // Clear the change tracker after the operation
        ClearChangeTracker();
        return changes > 0;
    }

    public async Task<bool> UpdateMany(List<T> data)
    {
        foreach (T entity in data)
        {
            if (entity == null) continue;

            DetachEntity(entity);
            T? existingEntity = await GetById(entity.Id);

            if (existingEntity == null) continue;

            await Update(entity);
        }

        int changes = await _context.SaveChangesAsync();
        ClearChangeTracker();
        return changes > 0;
    }

    //When you call await _context.SaveChangesAsync(), it returns an integer 
    //representing the number of rows affected by the changes you attempted to persist to the database.

    public async Task<bool> Add(T entity)
    {
        if (entity == null) return false;

        // Detach the entity from the context if it is already being tracked
        DetachEntity(entity);

        var existingEntity = await GetById(entity.Id!);
        if (existingEntity != null) return false;

        await DB.AddAsync(entity);
        var changes = await _context.SaveChangesAsync();

        // Clear the change tracker after the operation
        ClearChangeTracker();
        return changes > 0;
    }

    public virtual async Task<bool> Update(T entity)
    {
        if (entity == null) return false;

        // Detach the entity from the context if it is already being tracked
        DetachEntity(entity);

        // Check if the entity exists before updating
        var existingEntity = await GetById(entity.Id!);

        if (existingEntity == null) return false;

        DB.Update(entity);
        var changes = await _context.SaveChangesAsync();

        ClearChangeTracker();
        // Return true if the entity was successfully updated
        return changes > 0;
    }

    public async Task<bool> Remove(int id)
    {
        var entity = await GetById(id);
        // Retrieve entity to ensure it exists
        if (entity != null)
        {
            DB.Remove(entity);
            var changes = await _context.SaveChangesAsync();
            // Return true if the entity was successfully deleted
            return changes > 0;
        }
        return false;
    }

    public void DetachEntity(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            return;

        _context.Entry(entity).State = EntityState.Detached;
    }

    public void ClearChangeTracker()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            entry.State = EntityState.Detached;
        }
    }



    // Check if the table is empty
    public async Task<bool> IsTableEmpty()
    {
        return !await DB.AnyAsync();
    }
}