using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.FilmsManagement.Interfaces;
using Overoom.WEB.Contracts.FilmManagement;
using IFilmManagementMapper = Overoom.WEB.Mappers.Abstractions.IFilmManagementMapper;

namespace Overoom.WEB.Controllers;

[Authorize(Policy = "Admin")]
public class FilmManagementController : Controller
{
    private readonly IFilmManagementService _managementService;
    private readonly IFilmManagementMapper _mapper;

    public FilmManagementController(IFilmManagementService managementService, IFilmManagementMapper mapper)
    {
        _managementService = managementService;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult Index()
    {
        return View(new FilmsSearchParameters());
    }

    [HttpGet]
    public async Task<IActionResult> FilmsList(FilmsSearchParameters model)
    {
        if (!ModelState.IsValid) return NoContent();
        try
        {
            var films = await _managementService.FindAsync(model.Page, model.Query);
            if (!films.Any()) return NoContent();
            var filmsModels = films.Select(_mapper.Map).ToList();
            return Json(filmsModels);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var film = await _managementService.GetAsync(id);
        return View(_mapper.Map(film));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ChangeParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        if (model.NewPoster?.Length > 15728640)
        {
            ModelState.AddModelError("", "Размер постера не может превышать 15 Мб");
            return View(model);
        }

        await _managementService.ChangeAsync(_mapper.Map(model));
        return RedirectToAction("Index", "Home", new { message = "Фильм изменен" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid filmId)
    {
        await _managementService.DeleteAsync(filmId);
        return RedirectToAction("Index", "FilmManagement", new { message = "Фильм изменен" });
    }
}