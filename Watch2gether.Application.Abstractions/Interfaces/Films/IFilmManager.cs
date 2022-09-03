using Watch2gether.Application.Abstractions.DTO.Films.FilmCatalog;
using Watch2gether.Application.Abstractions.DTO.Playlists;

namespace Watch2gether.Application.Abstractions.Interfaces.Films;

public interface IFilmManager
{
    Task<List<FilmLiteDto>> GetFilms(FilmSearchQueryDto searchQueryDto);
    Task<FilmDto> GetFilm(Guid id);
}