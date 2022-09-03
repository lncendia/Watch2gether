using Microsoft.AspNetCore.Mvc;
using Watch2gether.Application.Abstractions.DTO.Films.FilmCatalog;
using Watch2gether.Application.Abstractions.DTO.Playlists;
using Watch2gether.Application.Abstractions.Interfaces.Films;
using Watch2gether.Application.Abstractions.Interfaces.Playlists;
using Watch2gether.WEB.Models.Film;
using SortBy = Watch2gether.Application.Abstractions.DTO.Playlists.SortBy;

namespace Watch2gether.WEB.Controllers;

public class FilmController : Controller
{
    private readonly IFilmManager _manager;
    private readonly IPlaylistManager _playlistManager;

    public FilmController(IFilmManager manager, IPlaylistManager playlistManager)
    {
        _manager = manager;
        _playlistManager = playlistManager;
    }

    public IActionResult Index(string? query = null, string? person = null, string? genre = null,
        string? country = null)
    {
        var model = new FilmsSearchViewModel {Query = query, Genre = genre, Country = country, Person = person};
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> FilmsList(FilmsSearchViewModel model)
    {
        if (!ModelState.IsValid) return NoContent();
        var films = await _manager.GetFilms(new FilmSearchQueryDto(model.Query, model.MinYear, model.MaxYear,
            model.Genre,
            model.Country, model.Person, model.Type, model.SortBy, model.Page, model.InverseOrder));
        if (!films.Any()) return NoContent();
        return Json(films.Select(x => new FilmLiteViewModel(x.Id, x.Name, x.PosterFileName, x.Rating,
            x.ShortDescription, x.Year, x.Type, x.CountSeasons, string.Join(", ", x.Genres))));
    }


    public async Task<IActionResult> Film(Guid id)
    {
        var film = await _manager.GetFilm(id);
        var playlists = await _playlistManager.GetPlaylists(new PlaylistSearchQueryDto(null, SortBy.Date, 1, false));
        var playlistViewModels =
            playlists.Select(x => new PlaylistLiteViewModel(x.Id, x.Name, x.PosterFileName)).ToList();
        var filmViewModel = new FilmViewModel(film.Id, film.Name, film.Year, film.Type, film.PosterFileName,
            film.Description,
            film.Rating, film.Directors, film.ScreenWriters, film.Genres, film.Countries, film.Actors,
            film.CountSeasons, film.CountEpisodes);

        return View(new FilmPageViewModel(filmViewModel, playlistViewModels));
    }
}