using Watch2gether.Application.Abstractions.DTO.Films.FilmCatalog;
using Watch2gether.Application.Abstractions.DTO.Playlists;

namespace Watch2gether.Application.Abstractions.Interfaces.Playlists;

public interface IPlaylistManager
{
    Task<List<PlaylistLiteDto>> GetPlaylists(PlaylistSearchQueryDto searchQueryDto);
    Task<PlaylistDto> GetPlaylist(Guid id);
}