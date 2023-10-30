using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;
using Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Playlists.Entities;

namespace Overoom.Application.Services.PlaylistsManagement;

public class PlaylistManagementMapper : IPlaylistManagementMapper
{
    public PlaylistDto MapGet(Playlist playlist, IEnumerable<Film> films)
    {
        var filmsDtos = films.OrderByDescending(x => x.Year)
            .Select(f => new FilmDto(f.Name, f.ShortDescription, f.Year, f.PosterUri, f.Id)).ToList();
        return new PlaylistDto(playlist.Id, playlist.Name, playlist.Description,
            playlist.PosterUri, filmsDtos);
    }

    public PlaylistShortDto MapShort(Playlist playlist) => new(playlist.Id, playlist.Name, playlist.PosterUri);
}