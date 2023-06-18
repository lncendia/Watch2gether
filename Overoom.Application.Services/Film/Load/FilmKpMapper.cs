using Overoom.Application.Abstractions.Films.Kinopoisk.DTOs;
using Overoom.Application.Abstractions.Films.Load.Interfaces;
using Overoom.Domain.Films.Enums;
using FilmDto = Overoom.Application.Abstractions.Films.Load.DTOs.FilmDto;
using FilmDtoBuilder = Overoom.Application.Abstractions.Films.Load.DTOs.FilmDtoBuilder;

namespace Overoom.Application.Services.Film.Load;

public class FilmKpMapper : IFilmKpMapper
{
    public FilmDto Map(Abstractions.Films.Kinopoisk.DTOs.Film film, FilmStaff staff,
        IReadOnlyCollection<Season>? seasons)
    {
        var builder = FilmDtoBuilder.Create()
            .WithName(film.Title)
            .WithYear(film.Year)
            .WithType(film.Serial ? FilmType.Serial : FilmType.Film)
            .WithPoster(film.PosterUrl)
            .WithDirectors(staff.Directors)
            .WithScreenwriters(staff.ScreenWriters)
            .WithGenres(film.Genres)
            .WithCountries(film.Countries)
            .WithActors(staff.Actors)
            .WithRatingKp(film.RatingKp!.Value)
            .WithDescription(film.Description!);

        if (!string.IsNullOrEmpty(film.ShortDescription)) builder = builder.WithShortDescription(film.ShortDescription);
        if (film.Serial && seasons != null)
            builder = builder.WithEpisodes(seasons.Count, seasons.Sum(x => x.Episodes.Count));
        return builder.Build();
    }
}