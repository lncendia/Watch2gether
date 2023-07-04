using Overoom.Application.Abstractions.Playlists.DTOs;

namespace Overoom.Application.Abstractions.Playlists.Interfaces;

public interface IPlaylistMapper
{
    PlaylistDto MapPlaylist(Domain.Playlists.Entities.Playlist playlist);
    PlaylistShortDto MapPlaylistShort(Domain.Playlists.Entities.Playlist playlist);
}