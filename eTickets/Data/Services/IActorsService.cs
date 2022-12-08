using eTickets.Models;

namespace eTickets.Data.Services;

public interface IActorsService
{
    public Task<IEnumerable<Actor>> GetAll();
    public Actor GetActorById(int id);
    public void Add(Actor actor);
    public Actor Updated(int id, Actor actor);
    public void Delete(int id);
}
