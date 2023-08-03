using Overoom.Application.Abstractions.FilmsInformation.DTOs;
using Overoom.Application.Abstractions.FilmsInformation.Interfaces;
using Overoom.Application.Abstractions.MovieApi.DTOs;
using Overoom.Domain.Films.Enums;
using CdnDto = Overoom.Application.Abstractions.FilmsInformation.DTOs.CdnDto;
using FilmDto = Overoom.Application.Abstractions.FilmsInformation.DTOs.FilmDto;

namespace Overoom.Application.Services.FilmsInformation;

public class FilmKpMapper : IFilmKpMapper
{
    public FilmDto Map(Film film, FilmStaff staff,
        IReadOnlyCollection<Season>? seasons, IReadOnlyCollection<Cdn> cdn)
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
            .WithDescription(film.Description!)
            .WithCdn(cdn.Select(x => new CdnDto(x.Type, x.Uri, x.Quality, x.Voices)).ToList());

        if (film.PosterUrl != null) builder = builder.WithPoster(film.PosterUrl);
        if (!string.IsNullOrEmpty(film.Description)) builder = builder.WithDescription(film.Description);
        if (!string.IsNullOrEmpty(film.ShortDescription)) builder = builder.WithShortDescription(film.ShortDescription);
        if (film.Serial && seasons != null)
        {
            var releasedSeasons = seasons.Where(x => x.Episodes.Any(e => e.ReleaseDate.HasValue)).ToList();
            builder = builder.WithEpisodes(releasedSeasons.Count, releasedSeasons.Sum(x => x.Episodes.Count));
        }
        
        return builder.Build();
    }
}