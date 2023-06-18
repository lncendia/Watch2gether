using Overoom.Application.Abstractions.Films.Load.DTOs;

namespace Overoom.Application.Abstractions.Films.Load.Interfaces;

public interface IFilmInfoService
{
    Task<FilmDto> GetFromTitleAsync(string title);
    Task<FilmDto> GetFromImdbAsync(string id);
    Task<FilmDto> GetFromKpAsync(long id);
}