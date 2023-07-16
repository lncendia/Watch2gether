using Overoom.Application.Abstractions.FilmsManagement.DTOs;

namespace Overoom.Application.Abstractions.FilmsManagement.Interfaces;

public interface IFilmInfoService
{
    Task<FilmDto> GetFromTitleAsync(string title);
    Task<FilmDto> GetFromImdbAsync(string id);
    Task<FilmDto> GetFromKpAsync(long id);
}