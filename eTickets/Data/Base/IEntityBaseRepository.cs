using eTickets.Models;
using System.Linq.Expressions;

namespace eTickets.Data.Base;

public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
    public Task<T> GetByIdAsync(int id);
    public Task AddAsync(T entity);
    public Task UpdateAsync(int id, T entity);
    public Task DeleteAsync(int id);
    public Task SaveChangesAsync();
}
