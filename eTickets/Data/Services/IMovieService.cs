using eTickets.Data.Base;
using eTickets.Data.ViewModels;
using eTickets.Models;

namespace eTickets.Data.Services;

public interface IMovieService : IEntityBaseRepository<Movie>
{
    public Task<Movie> GetMovieById(int id);
    public Task<MovieDropdownViewModel> GetMoviesDropdown();
    public Task AddNewMovieAsync(MovieViewModel movieViewModel);
}
