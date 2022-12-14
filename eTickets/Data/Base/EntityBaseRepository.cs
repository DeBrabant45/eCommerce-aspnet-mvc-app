using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eTickets.Data.Base;

public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
{
    private readonly AppDbContext _context;

    public EntityBaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FirstOrDefaultAsync(actor => actor.Id == id);

    public async Task UpdateAsync(int id, T entity)
    {
        var entityEntry = _context.Entry<T>(entity);
        entityEntry.State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(ent => ent.Id == id);
        if (entity == null)
            return;

        var entityEntry = _context.Entry<T>(entity);
        entityEntry.State = EntityState.Deleted;
        await SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _context.Set<T>();
        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        return await query.ToListAsync();
    }
}