using Overoom.Application.Abstractions.PlaylistsManagement.DTOs;
using Overoom.Domain.Films.Entities;
using Overoom.Domain.Playlists.Entities;

namespace Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;

public interface IPlaylistManagementMapper
{
    PlaylistDto MapGet(Playlist playlist, IEnumerable<Film> films);
    PlaylistShortDto MapShort(Playlist playlist);
}