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
}