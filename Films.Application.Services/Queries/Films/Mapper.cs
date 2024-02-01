using Films.Application.Abstractions.Queries.Films.DTOs;
using Films.Domain.Films.Entities;

namespace Films.Application.Services.Queries.Films;

internal class Mapper
{
    internal static FilmShortDto Map(Film film) => new()
    {
        Id = film.Id,
        Name = film.Title,
        PosterUrl = film.PosterUrl,
        Year = film.Year,
        Rating = film.UserRating,
        Description = film.ShortDescription!,
        Type = film.Type,
        Genres = film.Genres,
        CountSeasons = film.CountSeasons
    };
}