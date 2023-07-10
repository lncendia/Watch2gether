using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.Films.Load.Interfaces;
using Overoom.Domain.Films.Exceptions;
using Overoom.WEB.Contracts.FilmLoad;
using Overoom.WEB.Mappers.Abstractions;

namespace Overoom.WEB.Controllers;

[Authorize(Policy = "Admin")]
public class FilmLoadController : Controller
{
    private readonly IFilmLoadMapper _filmLoadMapper;
    private readonly IFilmLoadService _loadService;
    private readonly IFilmInfoService _filmInfoService;

    public FilmLoadController(IFilmLoadMapper filmLoadMapper, IFilmLoadService loadService,
        IFilmInfoService filmInfoService)
    {
        _filmLoadMapper = filmLoadMapper;
        _loadService = loadService;
        _filmInfoService = filmInfoService;
    }

    [HttpGet]
    public IActionResult Load()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Load(FilmLoadParameters model)
    {
        if (!ModelState.IsValid) return View(model);
        var film = _filmLoadMapper.Map(model);

        await _loadService.LoadAsync(film);
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