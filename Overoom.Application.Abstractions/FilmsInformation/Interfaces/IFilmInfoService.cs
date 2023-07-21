using Overoom.Application.Abstractions.FilmsInformation.DTOs;

namespace Overoom.Application.Abstractions.FilmsInformation.Interfaces;

public interface IFilmInfoService
{
    Task<FilmDto> GetFromTitleAsync(string title);
    Task<FilmDto> GetFromImdbAsync(string id);
    Task<FilmDto> GetFromKpAsync(long id);
}