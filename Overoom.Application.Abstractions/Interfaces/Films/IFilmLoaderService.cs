using Overoom.Application.Abstractions.DTO.Films.FilmLoader;

namespace Overoom.Application.Abstractions.Interfaces.Films;

public interface IFilmLoaderService
{
    Task<DownloaderResultDto> GetFilmsAsync(string? title, int page);
    Task DownloadFilmAsync(int id);
}