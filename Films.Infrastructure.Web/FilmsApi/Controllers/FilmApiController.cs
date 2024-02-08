using Films.Application.Abstractions.Queries.FilmsApi;
using Films.Application.Abstractions.Queries.FilmsApi.DTOs;
using Films.Infrastructure.Web.FilmsApi.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Films.Infrastructure.Web.FilmsApi.Controllers;

[ApiController]
[Authorize("admin")]
[Route("filmApi/[controller]/[action]")]
public class FilmApiController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<FilmApiViewModel> GetFromTitle(string title)
    {
        var film = await mediator.Send(new FindFilmByTitleQuery
        {
            Title = title
        });
        return Map(film);
    }

    [HttpGet]
    public async Task<FilmApiViewModel> GetFromKpId(long id)
    {
        var film = await mediator.Send(new FindFilmByKpIdQuery
        {
            Id = id
        });
        return Map(film);
    }

    [HttpGet]
    public async Task<FilmApiViewModel> GetFromImdb(string id)
    {
        var film = await mediator.Send(new FindFilmByTitleQuery
        {
            Title = id
        });
        return Map(film);
    }

    private static FilmApiViewModel Map(FilmApiDto film) => new()
    {
        Type = film.Type,
        Title = film.Title,
        Year = film.Year,
        Cdn = film.Cdn.Select(c => new CdnApiViewModel
        {
            Cdn = c.Name,
            Quality = c.Quality,
            Voices = c.Voices,
            Url = c.Url
        }),
        Countries = film.Countries,
        Actors = film.Actors.Select(a => new ActorApiViewModel
        {
            Name = a.Name,
            Description = a.Description
        }),
        Directors = film.Directors,
        Genres = film.Genres,
        Screenwriters = film.Screenwriters,
        CountSeasons = film.CountSeasons,
        CountEpisodes = film.CountEpisodes,
        RatingKp = film.RatingKp,
        RatingImdb = film.RatingImdb,
        Description = film.Description,
        ShortDescription = film.ShortDescription,
        PosterUrl = film.PosterUrl
    };
}