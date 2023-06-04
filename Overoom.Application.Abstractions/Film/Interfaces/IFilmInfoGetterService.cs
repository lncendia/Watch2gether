using Overoom.Application.Abstractions.Film.DTOs.FilmInfoGetter;

namespace Overoom.Application.Abstractions.Film.Interfaces;

public interface IFilmInfoGetterService
{
    Task<GetterResultDto> GetAsync(string? title, int page, int pageSize);
    Task<FilmFullInfoDto> GetFromVideoCdnIdAsync(int id);
}