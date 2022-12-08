using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services;

public class ActorsService : IActorsService
{
    private AppDbContext _context;

    public ActorsService(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Actor actor)
    {
        _context.Actors.Add(actor);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        _context.Remove(id);
    }

    public Actor GetActorById(int id)
    {
        return _context.Actors.Find(id);
    }

    async Task<IEnumerable<Actor>> IActorsService.GetAll()
    {
        var results = await _context.Actors.ToListAsync();
        return results;
    }

    public Actor Updated(int id, Actor actor)
    {
        return null;
    }
}
