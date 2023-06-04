using Overoom.Application.Abstractions.Film.DTOs.FilmCatalog;

namespace Overoom.Application.Abstractions.Film.Interfaces;

public interface IFilmManager
{
    Task<List<FilmShortDto>> FindAsync(FilmSearchQueryDto searchQueryDto);
    Task<FilmDto> GetAsync(Guid id);
    Task DeleteAsync(Guid id);
}