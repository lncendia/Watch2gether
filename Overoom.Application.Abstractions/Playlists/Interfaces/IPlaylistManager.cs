using Overoom.Application.Abstractions.Playlists.DTOs;

namespace Overoom.Application.Abstractions.Playlists.Interfaces;

public interface IPlaylistManager
{
    Task<List<PlaylistShortDto>> FindAsync(PlaylistSearchQueryDto searchQueryDto);
    Task<List<PlaylistShortDto>> FindByGenresAsync(IReadOnlyCollection<string> genres);
    Task<PlaylistDto> GetAsync(Guid id);
}