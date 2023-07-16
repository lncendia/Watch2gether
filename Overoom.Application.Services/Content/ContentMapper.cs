using Overoom.Application.Abstractions.Content.DTOs;
using Overoom.Application.Abstractions.Content.Interfaces;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Playlists.Entities;

namespace Overoom.Application.Services.Content;

public class ContentMapper : IContentMapper
{
    public FilmDto Map(Film film)
    {
        var description = !string.IsNullOrEmpty(film.ShortDescription)
            ? film.ShortDescription
            : film.Description[..100] + "...";

        return new FilmDto(film.Id, film.Name, film.PosterUri, film.Rating,
            description, film.Year, film.Type, film.CountSeasons, film.FilmTags.Genres);
    }

    public PlaylistDto Map(Playlist playlist) => new(playlist.Id, playlist.PosterUri, playlist.Updated,
        playlist.Name, playlist.Description, playlist.Genres);
}