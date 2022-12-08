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

    public async Task AddAsync(Actor actor)
    {
        await _context.Actors.AddAsync(actor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var actor = await _context.Actors.FirstOrDefaultAsync(actor => actor.Id == id);
        if (actor == null)
            return;

        _context.Actors.Remove(actor);
        await _context.SaveChangesAsync();
    }

    public async Task<Actor> GetActorByIdAsync(int id)
    {
        var results = await _context.Actors.FirstOrDefaultAsync(actor => actor.Id == id);
        return results;
    }

    public async Task<IEnumerable<Actor>> GetAllAsync()
    {
        var results = await _context.Actors.ToListAsync();
        return results;
    }

    public async Task<Actor> UpdateAsync(int id, Actor actor)
    {
        var actorToUpdated = await _context.Actors.FindAsync(id);

        actorToUpdated.ProfilePictureURL = actor.ProfilePictureURL;
        actorToUpdated.FullName = actor.FullName;
        actorToUpdated.Bio = actor.Bio;
        await _context.SaveChangesAsync();

        return actorToUpdated;
    }
}
