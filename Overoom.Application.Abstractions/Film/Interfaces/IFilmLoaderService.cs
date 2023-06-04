using Overoom.Application.Abstractions.Film.DTOs.FilmLoader;

namespace Overoom.Application.Abstractions.Film.Interfaces;

public interface IFilmLoaderService
{
    Task<DownloaderResultDto> GetAsync(string? title, int page);
    Task DownloadAsync(int id);
}