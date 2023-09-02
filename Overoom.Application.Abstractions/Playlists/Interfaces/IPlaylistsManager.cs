using Overoom.Application.Abstractions.Playlists.DTOs;

namespace Overoom.Application.Abstractions.Playlists.Interfaces;

public interface IPlaylistsManager
{
    Task<List<PlaylistDto>> FindAsync(int page = 1);
}