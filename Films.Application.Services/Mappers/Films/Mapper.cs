using Films.Application.Abstractions.DTOs.Films;
using Films.Domain.Films;

namespace Films.Application.Services.Mappers.Films;

internal class Mapper
{
    internal static FilmShortDto Map(Film film) => new()
    {
        Id = film.Id,
        Title = film.Title,
        PosterUrl = film.PosterUrl,
        Year = film.Year,
        UserRating = film.UserRating,
        RatingKp = film.RatingKp,
        RatingImdb = film.RatingImdb,
        Description = film.ShortDescription,
        IsSerial = film.IsSerial,
        Genres = film.Genres,
        CountSeasons = film.CountSeasons,
        CountEpisodes = film.CountEpisodes
    };
}