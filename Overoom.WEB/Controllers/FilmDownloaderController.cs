using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.DTO.Films.FilmLoader;
using Overoom.Application.Abstractions.Interfaces.Films;
using Overoom.WEB.Models.FilmDownloader;

namespace Overoom.WEB.Controllers;

[Authorize(Policy = "Admin")]
public class FilmDownloaderController : Controller
{
    private readonly IFilmLoaderService _filmLoaderService;

    public FilmDownloaderController(IFilmLoaderService filmLoaderService)
    {
        _filmLoaderService = filmLoaderService;
    }

    public IActionResult Index(string? title = null)
    {
        return View(new FilmsSearchViewModel {Title = title});
    }

    [HttpGet]
    public async Task<IActionResult> FilmsList(FilmsSearchViewModel model)
    {
        if (!ModelState.IsValid) return NoContent();
        DownloaderResultDto list;
        try
        {
            list = await _filmLoaderService.GetFilmsAsync(model.Title, model.Page ?? 1);
        }
        catch
        {
            return NoContent();
        }

        if (!list.Films.Any()) return NoContent();
        return Json(new FilmDownloaderViewModel(
            list.Films.Select(x => new FilmInfoViewModel(x.Id, x.Name, x.Year, x.Type)).ToList(), list.MoreAvailable));
    }

    [HttpPost]
    public async Task<IActionResult> AddFilm(int id)
    {
        if (id == 0) return BadRequest();
        try
        {
            await _filmLoaderService.DownloadFilmAsync(id);
            return Ok();
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}