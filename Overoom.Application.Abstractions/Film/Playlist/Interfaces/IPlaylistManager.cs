using Overoom.Application.Abstractions.Film.Playlist.DTOs;

namespace Overoom.Application.Abstractions.Film.Playlist.Interfaces;

public interface IPlaylistManager
{
    Task<List<PlaylistDto>> FindAsync(PlaylistSearchQueryDto searchQueryDto);
    Task<PlaylistDto> GetAsync(Guid id);
}