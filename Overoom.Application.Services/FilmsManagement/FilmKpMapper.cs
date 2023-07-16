using Overoom.Application.Abstractions.FilmsManagement.Interfaces;
using Overoom.Application.Abstractions.Kinopoisk.DTOs;
using Overoom.Domain.Films.Enums;
using FilmDto = Overoom.Application.Abstractions.FilmsManagement.DTOs.FilmDto;
using FilmDtoBuilder = Overoom.Application.Abstractions.FilmsManagement.DTOs.FilmDtoBuilder;

namespace Overoom.Application.Services.FilmsManagement;

public class FilmKpMapper : IFilmKpMapper
{
    public FilmDto Map(Film film, FilmStaff staff,
        IReadOnlyCollection<Season>? seasons)
    {
        var rating = film.Rating ?? film.RatingImdb;
        var builder = FilmDtoBuilder.Create()
            .WithName(film.Title)
            .WithYear(film.Year)
            .WithType(film.Serial ? FilmType.Serial : FilmType.Film)
            .WithDirectors(staff.Directors.Take(10).ToList())
            .WithScreenwriters(staff.ScreenWriters.Take(10).ToList())
            .WithGenres(film.Genres)
            .WithCountries(film.Countries)
            .WithActors(staff.Actors.Take(10).ToList())
            .WithRating(rating ?? 0)
            .WithDescription(film.Description!);

        if (film.PosterUrl != null) builder = builder.WithPoster(film.PosterUrl);
        if (!string.IsNullOrEmpty(film.Description)) builder = builder.WithDescription(film.Description);
        if (!string.IsNullOrEmpty(film.ShortDescription)) builder = builder.WithShortDescription(film.ShortDescription);
        if (film.Serial && seasons != null)
            builder = builder.WithEpisodes(seasons.Count, seasons.Sum(x => x.Episodes.Count));
        return builder.Build();
    }
}