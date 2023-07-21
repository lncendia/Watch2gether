using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.FilmsInformation.Interfaces;
using Overoom.Application.Abstractions.FilmsManagement.Interfaces;
using Overoom.WEB.Contracts.FilmLoad;
using Overoom.WEB.Mappers.Abstractions;

namespace Overoom.WEB.Controllers;

[Authorize(Policy = "Admin")]
public class FilmLoadController : Controller
{
    private readonly IFilmLoadMapper _filmLoadMapper;
    private readonly IFilmManagementService _managementService;
    private readonly IFilmInfoService _filmInfoService;

    public FilmLoadController(IFilmLoadMapper filmLoadMapper, IFilmManagementService managementService,
        IFilmInfoService filmInfoService)
    {
        _filmLoadMapper = filmLoadMapper;
        _managementService = managementService;
        _filmInfoService = filmInfoService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new LoadParameters());
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(LoadParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        if (model.Poster?.Length > 15728640)
        {
            ModelState.AddModelError("", "Размер постера не может превышать 15 Мб");
            return View(model);
        }

        var film = _filmLoadMapper.Map(model);

        await _managementService.LoadAsync(film);
        return RedirectToAction("Index", "Home", new { message = "Фильм успешно загружен" });
    }

    [HttpGet]
    public async Task<IActionResult> GetFromTitle(string title)
    {
        if (string.IsNullOrEmpty(title)) return BadRequest("Title required");
        try
        {
            var film = await _filmInfoService.GetFromTitleAsync(title);
            var model = _filmLoadMapper.Map(film);
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
            var model = _filmLoadMapper.Map(film);
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
            var model = _filmLoadMapper.Map(film);
            return Json(model);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}