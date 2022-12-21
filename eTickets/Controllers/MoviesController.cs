using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eTickets.Controllers;

public class MoviesController : Controller
{
    private readonly IMovieService _service;

    public MoviesController(IMovieService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var allMovies = await _service.GetAllAsync(n => n.Cinema);
        return View(allMovies);
    }    
    
    public async Task<IActionResult> Filter(string searchString)
    {
        var allMovies = await _service.GetAllAsync(n => n.Cinema);
        if (string.IsNullOrEmpty(searchString))
            return View(nameof(Index), allMovies);

        var filteredResult = allMovies.Where(n => n.Name.Contains(searchString) ||
            n.Description.Contains(searchString)).ToList();
        return View(nameof(Index), filteredResult);
    }

    public async Task<IActionResult> Details(int id)
    {
        var detail = await _service.GetMovieById(id);
        return View(detail);
    }

    public async Task<IActionResult> Create()
    {
        await CreateMovieDropdown();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(MovieViewModel movieViewModel)
    {
        if (!ModelState.IsValid)
        {
            await CreateMovieDropdown();
            return View(movieViewModel);
        }

        await _service.AddNewMovieAsync(movieViewModel);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var movie = await _service.GetMovieById(id);
        if (movie == null)
            return View("NotFound");

        var movieViewModel = await _service.SetMovieViewModel(movie);
        await CreateMovieDropdown();
        return View(movieViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, MovieViewModel movieViewModel)
    {
        if (id != movieViewModel.Id)
            return View("NotFound");

        if (!ModelState.IsValid)
        {
            await CreateMovieDropdown();
            return View(movieViewModel);
        }

        await _service.UpdateMovieAsync(movieViewModel);
        return RedirectToAction(nameof(Index));
    }

    private async Task CreateMovieDropdown()
    {
        var movieDropdown = await _service.GetMoviesDropdown();
        ViewBag.Cinemas = new SelectList(movieDropdown.Cinemas, "Id", "Name");
        ViewBag.Producers = new SelectList(movieDropdown.Producers, "Id", "FullName");
        ViewBag.Actors = new SelectList(movieDropdown.Actors, "Id", "FullName");
    }
}
