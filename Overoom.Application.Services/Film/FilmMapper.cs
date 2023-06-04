using Overoom.Application.Abstractions.Film.DTOs.FilmCatalog;
using Overoom.Application.Abstractions.Film.DTOs.Playlist;
using Overoom.Application.Abstractions.Film.Interfaces;
using Overoom.Domain.Film.Enums;

namespace Overoom.Application.Services.Film;

public class FilmMapper : IFilmMapper
{
    public FilmDto MapFilm(Domain.Film.Entities.Film film)
    {
        var x = FilmDtoBuilder.Create().WithId(film.Id)
            .WithName(film.Name)
            .WithDate(film.Date)
            .WithType(film.Type)
            .WithPoster(film.PosterFileName)
            .WithDescription(film.FilmInfo.Description)
            .WithRatingKp(film.FilmInfo.RatingKp)
            .WithUserRating(film.UserRating)
            .WithDirectors(film.FilmTags.Directors)
            .WithScreenwriters(film.FilmTags.Screenwriters)
            .WithGenres(film.FilmTags.Genres)
            .WithCountries(film.FilmTags.Countries)
            .WithActors(film.FilmTags.Actors.Select(x => (x.ActorName, x.ActorDescription)).ToList())
            .WithCdn(film.CdnList.Select(x => new CdnDto(x.Type, x.Uri, x.Quality, x.Voices)).ToList());

        if (film.Type == FilmType.Serial)
            x = x.WithEpisodes(film.FilmInfo.CountSeasons!.Value, film.FilmInfo.CountEpisodes!.Value);
        return x.Build();
    }

    public FilmShortDto MapFilmShort(Domain.Film.Entities.Film film)
    {
        var description = !string.IsNullOrEmpty(film.FilmInfo.ShortDescription)
            ? film.FilmInfo.ShortDescription
            : film.FilmInfo.Description[..100] + "...";

        return new FilmShortDto(film.Id, film.Name, film.PosterFileName, film.FilmInfo.RatingKp,
            description, film.Date.Year, film.Type, film.FilmInfo.CountSeasons, film.FilmTags.Genres);
    }
    
    public PlaylistDto MapPlaylist(Domain.Playlist.Entities.Playlist playlist)
    {
        return new PlaylistDto(playlist.Id, playlist.PosterFileName, playlist.Updated, playlist.Name,
            playlist.Description);
    }
}