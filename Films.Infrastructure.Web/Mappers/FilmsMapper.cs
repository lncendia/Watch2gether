using Films.Application.Abstractions.Films.DTOs;
using Films.Application.Abstractions.Queries.Films.Dtos;
using Films.Infrastructure.Web.Contracts.Films;
using Films.Infrastructure.Web.Mappers.Abstractions;
using Films.Infrastructure.Web.Models.Films;

namespace Films.Infrastructure.Web.Mappers;

public class FilmsMapper : IFilmsMapper
{
    public FilmsSearchParameters Map(SearchParameters model) =>
        new()
        {
            Genre = model.Genre, Country = model.Country, Person = model.Person, Query = model.Title, Type = model.Type,
            PlaylistId = model.PlaylistId
        };

    public FilmSearchQuery Map(FilmsSearchParameters model) =>
        new(model.Query, model.Genre, model.Country, model.Person, model.Type, model.PlaylistId, model.Page);

    public FilmViewModel Map(FilmDto film) => new(film.Id, film.Name, film.PosterUri, film.Rating,
        film.ShortDescription, film.Year, film.Type, film.CountSeasons, film.Genres);

    public FilmShortViewModel MapShort(FilmDto film) => new(film.Id, film.Name, film.PosterUri, film.Year);
}