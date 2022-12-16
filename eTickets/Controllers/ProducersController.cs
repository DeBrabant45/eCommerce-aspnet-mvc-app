using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Controllers;
public class ProducersController : Controller
{
    private readonly IProducersService _service;

    public ProducersController(IProducersService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _service.GetAllAsync();
        return View(data);
    }

    public async Task<IActionResult> Details(int id)
    {
        var details = await _service.GetByIdAsync(id);
        if (details == null)
            return View("NotFound");

        return View(details);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind(nameof(producer.FullName), nameof(producer.ProfilePictureURL), nameof(producer.Bio))] Producer producer)
    {
        if (!ModelState.IsValid)
            return View(producer);

        await _service.AddAsync(producer);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var producer = await _service.GetByIdAsync(id);
        if (producer == null)
            return View("NotFound");

        return View(producer);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind(nameof(producer.Id), nameof(producer.FullName), nameof(producer.ProfilePictureURL), nameof(producer.Bio))] Producer producer)
    {
        if (!ModelState.IsValid)
            return View(producer);

        if (id != producer.Id)
            return View(producer);

        await _service.UpdateAsync(id, producer);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var producer = await _service.GetByIdAsync(id);
        if (producer == null)
            return View("NotFound");

        return View(producer);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producer = await _service.GetByIdAsync(id);
        if (producer == null)
            return View("NotFound");

        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
