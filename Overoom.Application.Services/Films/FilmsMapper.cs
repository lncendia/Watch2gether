using Overoom.Application.Abstractions.Films.DTOs;
using Overoom.Application.Abstractions.Films.Interfaces;
using Overoom.Domain.Films.Entities;

namespace Overoom.Application.Services.Films;

public class FilmsMapper : IFilmsMapper
{
    public FilmDto Map(Film film)
    {
        var description = !string.IsNullOrEmpty(film.ShortDescription)
            ? film.ShortDescription
            : film.Description[..100] + "...";

        return new FilmDto(film.Id, film.Name, film.PosterUri, film.Rating,
            description, film.Year, film.Type, film.CountSeasons, film.FilmTags.Genres);
    }
}