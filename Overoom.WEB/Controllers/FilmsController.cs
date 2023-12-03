﻿using Microsoft.AspNetCore.Mvc;
using Overoom.Application.Abstractions.Films.Interfaces;
using Overoom.WEB.Contracts.Films;
using Overoom.WEB.Models.Films;
using IFilmsMapper = Overoom.WEB.Mappers.Abstractions.IFilmsMapper;

namespace Overoom.WEB.Controllers;

public class FilmsController : Controller
{
    private readonly IFilmsManager _filmsManager;
    private readonly IFilmsMapper _filmsMapper;

    public FilmsController(IFilmsMapper filmsMapper, IFilmsManager filmsManager)
    {
        _filmsMapper = filmsMapper;
        _filmsManager = filmsManager;
    }

    public async Task<IActionResult> Films()
    {
        var popular = await _filmsManager.PopularFilmsAsync();
        var vm = new FilmsViewModel(popular.Select(_filmsMapper.MapShort).ToList());
        return View(vm);
    }

    public async Task<IActionResult> FilmSearch(SearchParameters model)
    {
        var search = _filmsMapper.Map(model);

        if (search.PlaylistId.HasValue)
            ViewData["playlist"] = await _filmsManager.GetPlaylistNameAsync(search.PlaylistId.Value);
        return View(search);
    }

    [HttpGet]
    public async Task<IActionResult> FilmsList(FilmsSearchParameters model)
    {
        if (!ModelState.IsValid) return NoContent();
        try
        {
            var films = await _filmsManager.FindAsync(_filmsMapper.Map(model));
            if (!films.Any()) return NoContent();
            var filmsModels = films.Select(_filmsMapper.Map).ToList();
            return Json(filmsModels);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}