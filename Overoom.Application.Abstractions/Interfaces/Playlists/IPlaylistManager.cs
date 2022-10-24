using Overoom.Application.Abstractions.DTO.Films.FilmCatalog;
using Overoom.Application.Abstractions.DTO.Playlists;

namespace Overoom.Application.Abstractions.Interfaces.Playlists;

public interface IPlaylistManager
{
    Task<List<PlaylistLiteDto>> GetPlaylists(PlaylistSearchQueryDto searchQueryDto);
    Task<PlaylistDto> GetPlaylist(Guid id);
}