using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public abstract class BaseAccess<T> where T : class, IHasId
{
    protected readonly DbContext _context;
    private DbSet<T> DB;

    public BaseAccess(DbContext context)
    {
        _context = context;
        DB = _context.Set<T>();
    }

    public async Task<List<T>> GetAll()
    {
        return await DB.ToListAsync();
    }

    public async Task<T> GetById(int id)
    {
        return await DB.FirstOrDefaultAsync(entity => entity.Id == id);
    }


    //When you call await _context.SaveChangesAsync(), it returns an integer 
    //representing the number of rows affected by the changes you attempted to persist to the database.

    public async Task<bool> Add(T entity)
    {
        if (entity == null) return false;
        var existingEntity = await GetById(entity.Id);
        if (existingEntity != null)
        {
            return false;
        }
        await DB.AddAsync(entity);
        var changes = await _context.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<bool> Update(T entity)
    {
        if (entity == null) return false;

        // Check if the entity exists before updating
        var existingEntity = await GetById(entity.Id);
        if (existingEntity == null)
        {
            return false;
        }
        DB.Update(entity);
        var changes = await _context.SaveChangesAsync();

        // Return true if the entity was successfully updated
        return changes > 0;
    }

    public async Task<bool> Delete(int id)
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
}