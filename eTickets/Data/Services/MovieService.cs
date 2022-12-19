using eTickets.Data.Base;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services;

public class MovieService : EntityBaseRepository<Movie>, IMovieService
{
    private readonly AppDbContext _context;
    public MovieService(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task AddNewMovieAsync(MovieViewModel movieViewModel)
    {
        var movie = new Movie
        {
            Name = movieViewModel.Name,
            Description = movieViewModel.Description,
            Price = movieViewModel.Price,
            ImageUrl = movieViewModel.ImageUrl,
            StartDate = movieViewModel.StartDate,
            EndDate = movieViewModel.EndDate,
            MovieCategory = movieViewModel.MovieCategory,
            ProducerId = movieViewModel.ProducerId,
            CinemaId = movieViewModel.CinemaId,
        };
        await _context.AddAsync(movie);
        await SaveChangesAsync();

        movieViewModel.ActorIds.ForEach(async actorId =>
        {
            var newActorMovie = new Actor_Movie()
            {
                MovieId = movie.Id,
                ActorId = actorId,
            };
            await _context.Actors_Movies.AddAsync(newActorMovie);
        });
        await SaveChangesAsync();
    }

    public async Task<Movie> GetMovieById(int id)
    {
        var details = await _context.Movies
            .Include(c => c.Cinema)
            .Include(p => p.Producer)
            .Include(am => am.Actors_Movies)
            .ThenInclude(a => a.Actor)
            .FirstOrDefaultAsync(n => n.Id == id);

        return details;
    }

    public async Task<MovieDropdownViewModel> GetMoviesDropdown()
    {
        var movieDropdown = new MovieDropdownViewModel()
        {
            Actors = await _context.Actors.OrderBy(actor => actor.FullName).ToListAsync(),
            Cinemas = await _context.Cinemas.OrderBy(cinema => cinema.Name).ToListAsync(),
            Producers = await _context.Producers.OrderBy(producer => producer.FullName).ToListAsync(),
        };

        return movieDropdown;
    }
}
