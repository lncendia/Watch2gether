using Overoom.Application.Abstractions.Films.Catalog.DTOs;

namespace Overoom.Application.Abstractions.Films.Catalog.Interfaces;

public interface IFilmManager
{
    Task<List<FilmShortDto>> FindAsync(FilmSearchQueryDto searchQueryDto);
    Task<FilmDto> GetAsync(Guid id);
}