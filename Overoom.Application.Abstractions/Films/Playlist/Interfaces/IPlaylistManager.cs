using Overoom.Application.Abstractions.Films.Playlist.DTOs;

namespace Overoom.Application.Abstractions.Films.Playlist.Interfaces;

public interface IPlaylistManager
{
    Task<List<PlaylistShortDto>> FindAsync(PlaylistSearchQueryDto searchQueryDto);
    Task<List<PlaylistShortDto>> FindByGenresAsync(IReadOnlyCollection<string> genres);
    Task<PlaylistDto> GetAsync(Guid id);
}