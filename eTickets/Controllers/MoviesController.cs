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
        var data = await _service.GetAllAsync(n => n.Cinema);
        return View(data);
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

    private async Task CreateMovieDropdown()
    {
        var movieDropdown = await _service.GetMoviesDropdown();
        ViewBag.Cinemas = new SelectList(movieDropdown.Cinemas, "Id", "Name");
        ViewBag.Producers = new SelectList(movieDropdown.Producers, "Id", "FullName");
        ViewBag.Actors = new SelectList(movieDropdown.Actors, "Id", "FullName");
    }
}
