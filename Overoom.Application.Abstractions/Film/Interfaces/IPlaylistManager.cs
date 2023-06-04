using Overoom.Application.Abstractions.Film.DTOs.FilmCatalog;
using Overoom.Application.Abstractions.Film.DTOs.Playlist;

namespace Overoom.Application.Abstractions.Film.Interfaces;

public interface IPlaylistManager
{
    Task<List<PlaylistDto>> FindAsync(PlaylistSearchQueryDto searchQueryDto);
    Task<PlaylistDto> GetAsync(Guid id);
    Task<List<FilmShortDto>> FindFilmsAsync(FilmSearchQueryDto searchQueryDto);
}