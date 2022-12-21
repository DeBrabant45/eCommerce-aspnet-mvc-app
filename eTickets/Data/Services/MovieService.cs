using eTickets.Data.Base;
using eTickets.Data.Enums;
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
        var movie = SetMovie(movieViewModel);
        await _context.AddAsync(movie);
        await SaveChangesAsync();
        await AddMovieActorIds(movieViewModel.ActorIds, movie.Id);
    }

    private async Task AddMovieActorIds(List<int> ActorIds, int movieId)
    {
        ActorIds.ForEach(async actorId =>
        {
            var newActorMovie = new Actor_Movie()
            {
                MovieId = movieId,
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

    public Task<MovieViewModel> SetMovieViewModel(Movie movie)
    {
        return Task.FromResult(new MovieViewModel
        {
            Id = movie.Id,
            Name = movie.Name,
            Description = movie.Description,
            Price = movie.Price,
            ImageUrl = movie.ImageUrl,
            StartDate = movie.StartDate,
            EndDate = movie.EndDate,
            MovieCategory = movie.MovieCategory,
            CinemaId = movie.CinemaId,
            ProducerId = movie.ProducerId,
            ActorIds = movie.Actors_Movies.Select(n => n.ActorId).ToList(),
        });
    }

    public async Task UpdateMovieAsync(MovieViewModel movieViewModel)
    {
        var movie = await GetByIdAsync(movieViewModel.Id);
        if (movie == null)
            return;

        movie.Name = movieViewModel.Name;
        movie.Description = movieViewModel.Description;
        movie.Price = movieViewModel.Price;
        movie.ImageUrl = movieViewModel.ImageUrl;
        movie.StartDate = movieViewModel.StartDate;
        movie.EndDate = movieViewModel.EndDate;
        movie.MovieCategory = movieViewModel.MovieCategory;
        movie.ProducerId = movieViewModel.ProducerId;
        movie.CinemaId = movieViewModel.CinemaId;
        await SaveChangesAsync();
        var existingActors = await _context.Actors_Movies.Where(n => n.MovieId == movieViewModel.Id).ToListAsync();
        _context.Actors_Movies.RemoveRange(existingActors);
        await SaveChangesAsync();
        await AddMovieActorIds(movieViewModel.ActorIds, movieViewModel.Id);
    }

    private Movie SetMovie(MovieViewModel movieViewModel)
    {
        return new Movie
        {
            Id = movieViewModel.Id,
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
    }
}
