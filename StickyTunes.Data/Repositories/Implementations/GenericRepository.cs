using Microsoft.EntityFrameworkCore;
using StickyTunes.Data.Contexts;

namespace StickyTunes.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly StickyTunesDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(StickyTunesDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return false;
        
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }
}
