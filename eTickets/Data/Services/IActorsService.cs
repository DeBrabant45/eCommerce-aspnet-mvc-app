using eTickets.Models;

namespace eTickets.Data.Services;

public interface IActorsService
{
    public Task<IEnumerable<Actor>> GetAllAsync();
    public Task<Actor> GetActorByIdAsync(int id);
    public Task AddAsync(Actor actor);
    public Task<Actor> UpdateAsync(int id, Actor actor);
    public Task DeleteAsync(int id);
}
