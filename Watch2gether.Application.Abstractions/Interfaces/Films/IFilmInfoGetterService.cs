using Watch2gether.Application.Abstractions.DTO.Films.FilmInfoGetter;

namespace Watch2gether.Application.Abstractions.Interfaces.Films;

public interface IFilmInfoGetterService
{
    Task<GetterResultDto> GetFilmsAsync(string? title, int page, int pageSize);
    Task<FilmFullInfoDto> GetFilmFromVideoCdnIdAsync(int id);
}