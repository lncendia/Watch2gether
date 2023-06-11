using Overoom.Application.Abstractions.Film.Catalog.DTOs;

namespace Overoom.Application.Abstractions.Film.Catalog.Interfaces;

public interface IFilmManager
{
    Task<List<FilmShortDto>> FindAsync(FilmSearchQueryDto searchQueryDto);
    Task<FilmDto> GetAsync(Guid id);
}