using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.Content.DTOs;
using Overoom.Application.Abstractions.Content.Interfaces;
using Overoom.WEB.Contracts.Content;
using IContentMapper = Overoom.WEB.Mappers.Abstractions.IContentMapper;

namespace Overoom.WEB.Controllers;

public class ContentController : Controller
{
    private readonly IContentManager _contentManager;
    private readonly IContentMapper _contentMapper;

    public ContentController(IContentMapper contentMapper, IContentManager contentManager)
    {
        _contentMapper = contentMapper;
        _contentManager = contentManager;
    }

    public IActionResult Films(SearchParameters model)
    {
        var search = new FilmsSearchParameters
        {
            Genre = model.Genre, Country = model.Country, Person = model.Person, Query = model.Title,
            SortBy = FilmSortBy.Date, PlaylistId = model.PlaylistId
        };
        return View(search);
    }

    public IActionResult Playlists()
    {
        return View(new PlaylistsSearchParameters());
    }

    [HttpGet]
    public async Task<IActionResult> FilmsList(FilmsSearchParameters model)
    {
        if (!ModelState.IsValid) return NoContent();
        try
        {
            var films = await _contentManager.FindAsync(_contentMapper.Map(model));
            if (!films.Any()) return NoContent();
            var filmsModels = films.Select(_contentMapper.Map).ToList();
            return Json(filmsModels);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> PlaylistsList(PlaylistsSearchParameters model)
    {
        if (!ModelState.IsValid) return NoContent();
        try
        {
            var films = await _contentManager.FindAsync(_contentMapper.Map(model));
            if (!films.Any()) return NoContent();
            var filmsModels = films.Select(_contentMapper.Map).ToList();
            return Json(filmsModels);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}