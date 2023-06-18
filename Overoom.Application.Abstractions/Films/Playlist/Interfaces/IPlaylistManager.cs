using Overoom.Application.Abstractions.Films.Playlist.DTOs;

namespace Overoom.Application.Abstractions.Films.Playlist.Interfaces;

public interface IPlaylistManager
{
    Task<List<PlaylistDto>> FindAsync(PlaylistSearchQueryDto searchQueryDto);
    Task<PlaylistDto> GetAsync(Guid id);
}