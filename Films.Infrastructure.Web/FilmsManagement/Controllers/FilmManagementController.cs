using Films.Application.Abstractions.EventHandlers.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Films.Application.Abstractions.FilmsInformation.Interfaces;
using Films.Application.Abstractions.FilmsManagement.Interfaces;
using Films.Infrastructure.Web.Contracts.FilmManagement;
using Films.Infrastructure.Web.Contracts.FilmManagement.Load;
using Films.Domain.Films.Exceptions;
using Abstractions_IFilmManagementMapper = Films.Infrastructure.Web.Mappers.Abstractions.IFilmManagementMapper;
using IFilmManagementMapper = Films.Infrastructure.Web.Mappers.Abstractions.IFilmManagementMapper;
using Mappers_Abstractions_IFilmManagementMapper = Films.Infrastructure.Web.Mappers.Abstractions.IFilmManagementMapper;

namespace Films.Infrastructure.Web.Controllers;

[Authorize(Policy = "Admin")]
public class FilmManagementController : Controller
{
    private readonly IFilmManagementService _managementService;
    private readonly Mappers_Abstractions_IFilmManagementMapper _mapper;
    private readonly IFilmInfoService _filmInfoService;

    public FilmManagementController(IFilmManagementService managementService, Mappers_Abstractions_IFilmManagementMapper mapper,
        IFilmInfoService filmInfoService)
    {
        _managementService = managementService;
        _mapper = mapper;
        _filmInfoService = filmInfoService;
    }

    [HttpGet]
    public ActionResult Index()
    {
        return View(new FilmsSearchInputModel());
    }

    [HttpGet]
    public async Task<IActionResult> FilmsList(FilmsSearchInputModel model)
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
    public IActionResult Load()
    {
        return View(new AddFilmInputModel());
    }

    [HttpPost]
    public async Task<IActionResult> Load(AddFilmInputModel model)
    {
        if (!ModelState.IsValid) return View(model);
        if (model.Poster?.Length > 15728640)
        {
            ModelState.AddModelError("", "Размер постера не может превышать 15 Мб");
            return View(model);
        }

        var film = _mapper.Map(model);

        try
        {
            await _managementService.LoadAsync(film);
            return RedirectToAction("Index", "Home", new { message = "Фильм успешно загружен" });
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case DuplicateCdnException:
                    ModelState.AddModelError("",
                        "CDN не могут повторяться");
                    break;
                case SerialException:
                    ModelState.AddModelError("",
                        "Для сериалов обязательно количество серий и эпизодов");
                    break;
                case FilmAlreadyExistsException:
                    ModelState.AddModelError("",
                        "Данный фильм уже присутствует в библиотеке");
                    break;
                default: throw;
            }

            return View(model);
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
    public async Task<IActionResult> Edit(ChangeFilmInputModel model)
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
        return RedirectToAction("Index", "FilmManagement", new { message = "Фильм удалён" });
    }

    [HttpGet]
    public async Task<IActionResult> GetFromTitle(string title)
    {
        if (string.IsNullOrEmpty(title)) return BadRequest("Title required");
        try
        {
            var film = await _filmInfoService.GetFromTitleAsync(title);
            var model = _mapper.Map(film);
            return Json(model);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetFromKpId(long id)
    {
        try
        {
            var film = await _filmInfoService.GetFromKpAsync(id);
            var model = _mapper.Map(film);
            return Json(model);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetFromImdb(string id)
    {
        if (string.IsNullOrEmpty(id)) return BadRequest("Title required");
        try
        {
            var film = await _filmInfoService.GetFromImdbAsync(id);
            var model = _mapper.Map(film);
            return Json(model);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}