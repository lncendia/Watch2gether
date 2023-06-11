using Overoom.Application.Abstractions.Film.Load.DTOs;

namespace Overoom.Application.Abstractions.Film.Load.Interfaces;

public interface IFilmInfoService
{
    Task<FilmDto> GetFromTitleAsync(string title);
    Task<FilmDto> GetFromImdbAsync(string id);
    Task<FilmDto> GetFromKpAsync(long id);
}