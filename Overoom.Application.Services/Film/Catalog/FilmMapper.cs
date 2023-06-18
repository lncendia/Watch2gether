using Overoom.Application.Abstractions.Films.Catalog.DTOs;
using Overoom.Application.Abstractions.Films.Catalog.Interfaces;
using Overoom.Domain.Films.Enums;

namespace Overoom.Application.Services.Film.Catalog;

public class FilmMapper : IFilmMapper
{
    public FilmDto MapFilm(Domain.Films.Entities.Film film)
    {
        var x = FilmDtoBuilder.Create()
            .WithId(film.Id)
            .WithName(film.Name)
            .WithYear(film.Year)
            .WithType(film.Type)
            .WithPoster(film.PosterUri)
            .WithDescription(film.Description)
            .WithRatingKp(film.RatingKp)
            .WithUserRating(film.UserRating)
            .WithDirectors(film.FilmTags.Directors)
            .WithScreenwriters(film.FilmTags.Screenwriters)
            .WithGenres(film.FilmTags.Genres)
            .WithCountries(film.FilmTags.Countries)
            .WithActors(film.FilmTags.Actors.Select(x => (x.ActorName, x.ActorDescription)).ToList())
            .WithCdn(film.CdnList.Select(x => new CdnDto(x.Type, x.Uri, x.Quality, x.Voices)).ToList());

        if (film.Type == FilmType.Serial)
            x = x.WithEpisodes(film.CountSeasons!.Value, film.CountEpisodes!.Value);
        return x.Build();
    }

    public FilmShortDto MapFilmShort(Domain.Films.Entities.Film film)
    {
        var description = !string.IsNullOrEmpty(film.ShortDescription)
            ? film.ShortDescription
            : film.Description[..100] + "...";

        return new FilmShortDto(film.Id, film.Name, film.PosterUri, film.RatingKp,
            description, film.Year, film.Type, film.CountSeasons, film.FilmTags.Genres);
    }
}