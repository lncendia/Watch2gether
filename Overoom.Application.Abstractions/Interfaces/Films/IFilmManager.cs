using Overoom.Application.Abstractions.DTO.Playlists;
using Overoom.Application.Abstractions.DTO.Films.FilmCatalog;

namespace Overoom.Application.Abstractions.Interfaces.Films;

public interface IFilmManager
{
    Task<List<FilmLiteDto>> GetFilmsAsync(FilmSearchQueryDto searchQueryDto);
    Task<FilmDto> GetFilmAsync(Guid id);
    Task DeleteFilmAsync(Guid id);
}