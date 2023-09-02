using Overoom.Application.Abstractions.Films.DTOs;
using Overoom.WEB.Contracts.Films;
using Overoom.WEB.Mappers.Abstractions;
using Overoom.WEB.Models.Films;

namespace Overoom.WEB.Mappers;

public class FilmsMapper : IFilmsMapper
{
    public FilmSearchQuery Map(FilmsSearchParameters model) =>
        new(model.Query, model.Genre, model.Country, model.Person, model.Type, model.PlaylistId, model.Page);

    public FilmViewModel Map(FilmDto film) => new(film.Id, film.Name, film.PosterUri, film.Rating,
        film.ShortDescription, film.Year, film.Type, film.CountSeasons, film.Genres);

    public FilmShortViewModel MapShort(FilmDto film) => new(film.Id, film.Name, film.PosterUri, film.Year);
}