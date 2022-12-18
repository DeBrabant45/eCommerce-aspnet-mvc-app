using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers;

public class CinemasController : Controller
{
    private readonly ICinemasService _service;

    public CinemasController(ICinemasService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _service.GetAllAsync();
        return View(data);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind(nameof(cinema.LogoUrl), nameof(cinema.Name), nameof(cinema.Description))] Cinema cinema)
    {
        if (!ModelState.IsValid)
            return View(cinema);

        await _service.AddAsync(cinema);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var details = await _service.GetByIdAsync(id);
        if (details == null)
            return View("NotFound");

        return View(details);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var cinema = await _service.GetByIdAsync(id);
        if (cinema == null)
            return View("NotFound");

        return View(cinema);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind( nameof(cinema.Id), nameof(cinema.LogoUrl), nameof(cinema.Name), nameof(cinema.Description))] Cinema cinema)
    {
        if (!ModelState.IsValid)
            return View(cinema);

        await _service.UpdateAsync(id, cinema);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var cinema = await _service.GetByIdAsync(id);
        if (cinema == null)
            return View("NotFound");

        return View(cinema);
    }

    [HttpPost]
    public async Task<IActionResult> DeletePost(int id)
    {
        var producer = await _service.GetByIdAsync(id);
        if (producer == null)
            return View("NotFound");

        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
